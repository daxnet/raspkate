using Newtonsoft.Json;
using Raspkate.Controllers;
using Raspkate.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspkate.Handlers
{
    internal sealed class ControllerHandler : RaspkateHandler
    {
        private readonly Regex fileNameRegularExpression = new Regex(FileHandler.Pattern);
        private readonly List<ControllerRegistration> controllerRegistrations = new List<ControllerRegistration>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ControllerHandler(RaspkateServer server, IEnumerable<KeyValuePair<string, string>> properties)
            : base(server, properties)
        { }

        protected internal override void OnRegistering()
        {
            var controllerTypeNames = GetPropertyValue("Controllers").Split(';').Select(p => p.Trim());
            foreach (var controllerTypeName in controllerTypeNames)
            {
                var controllerType = Type.GetType(controllerTypeName);
                if (controllerType == null)
                {
                    log.WarnFormat("Cannot identify the controller type with the name \"{0}\", skipping...", controllerTypeName);
                    continue;
                }
                if (!controllerType.IsSubclassOf(typeof(RaspkateController)))
                {
                    log.WarnFormat("Type \"{0}\" is not a valid Raspkate controller, skipping...", controllerTypeName);
                    continue;
                }
                this.RegisterControllerType(controllerType);
                log.InfoFormat("Controller type \"{0}\" registered successfully.", controllerTypeName);
            }
        }

        public override bool ShouldHandle(HttpListenerRequest request)
        {
            return request.RawUrl != "/" &&
                !this.fileNameRegularExpression.Match(request.RawUrl.Trim('\\', '/', '?')).Success;
        }

        public override void Process(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                var requestedUri = request.RawUrl.Trim('/');
                foreach(var controllerRegistration in this.controllerRegistrations)
                {
                    // Checks the HTTP method.
                    var httpMethodName = controllerRegistration.ControllerMethod.GetCustomAttribute<HttpGetAttribute>().MethodName;
                    if (request.HttpMethod != httpMethodName)
                    {
                        continue;
                    }

                    // Checks if the current controller registration matches the requested route.
                    RouteValueCollection values;
                    if (controllerRegistration.Route.TryGetValue(requestedUri, out values))
                    {
                        // If successfully get the route values, then bind the parameter.
                        List<object> parameterValues = new List<object>();
                        foreach(var parameter in controllerRegistration.ControllerMethod.GetParameters())
                        {
                            if (parameter.IsDefined(typeof(FromBodyAttribute)))
                            {
                                if (controllerRegistration.ControllerMethod.IsDefined(typeof(HttpGetAttribute)))
                                {
                                    throw new ControllerException("Parameter \"{0}\" of method {1}.{2} has the FromBodyAttribute defined, which is not allowed in an HTTP GET method.", parameter.Name, controllerRegistration.ControllerType.Name, controllerRegistration.ControllerMethod.Name);
                                }
                                else if (controllerRegistration.ControllerMethod.IsDefined(typeof(HttpPostAttribute)))
                                {
                                    var bodyContent = string.Empty;
                                    if (request.ContentLength64 > 0)
                                    {
                                        var bytes = new byte[request.ContentLength64];
                                        request.InputStream.Read(bytes, 0, (int)request.ContentLength64);
                                        bodyContent = request.ContentEncoding.GetString(bytes);
                                    }
                                    parameterValues.Add(JsonConvert.DeserializeObject(bodyContent));
                                }
                            }

                            if (values.ContainsKey(parameter.Name))
                            {
                                parameterValues.Add(values[parameter.Name]);
                            }
                            else
                            {
                                throw new ControllerException("Parameter binding failed: Unrecognized parameter \"{0}\" defined in the controller method {1}.{2}.", 
                                    parameter.Name, 
                                    controllerRegistration.ControllerType.Name, 
                                    controllerRegistration.ControllerMethod.Name);
                            }
                        }

                        // Call the controller method
                        var controller = (RaspkateController)Activator.CreateInstance(controllerRegistration.ControllerType);
                        if (controller != null)
                        {
                            if (controllerRegistration.ControllerMethod.ReturnType == typeof(void))
                            {
                                controllerRegistration.ControllerMethod.Invoke(controller, parameterValues.ToArray());
                                response.StatusCode = (int)HttpStatusCode.OK;
                            }
                            else
                            {
                                var responseString = JsonConvert.SerializeObject(controllerRegistration.ControllerMethod.Invoke(controller, parameterValues.ToArray()));
                                response.WriteResponse(HttpStatusCode.OK, "application/json", responseString);
                            }
                        }
                    }
                }
                throw new ControllerException("No registered controller can handle the request with route \"{0}\".", requestedUri);
            }
            catch (ControllerException ex)
            {
                log.Warn("Unable to proceed with the given request.", ex);
                response.WriteResponse(HttpStatusCode.BadRequest, ex);
            }
            catch(Exception ex)
            {
                log.Error("Error occurred when processing the request.", ex);
                response.WriteResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private void RegisterControllerType(Type controllerType)
        {
            string routePrefix = string.Empty;
            if (controllerType.IsDefined(typeof(RoutePrefixAttribute)))
            {
                routePrefix = (controllerType.GetCustomAttributes(typeof(RoutePrefixAttribute), false).First() as RoutePrefixAttribute).Prefix;
            }

            var methodQuery = from m in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                              where m.IsDefined(typeof(HttpMethodAttribute), true)
                              select new
                              {
                                  Route = m.IsDefined(typeof(RouteAttribute)) ? m.GetCustomAttribute<RouteAttribute>().Name : m.Name,
                                  MethodInfo = m
                              };
            foreach (var methodQueryItem in methodQuery)
            {
                string routeString = string.Empty;
                Route route;
                if (methodQueryItem.Route.StartsWith("!"))
                {
                    routeString = methodQueryItem.Route.Substring(1);
                }
                else
                {
                    routeString = routePrefix;
                    if (!string.IsNullOrEmpty(routeString) && !routeString.EndsWith("/"))
                        routeString += "/";
                    routeString += methodQueryItem.Route;
                }
                try
                {
                    route = RouteParser.Parse(routeString);
                }
                catch (RouteParseException rpe)
                {
                    log.Warn(string.Format("Route parsing failed, ignoring the decorated controller method. (Route: \"{0}\", Method:{1}.{2})", 
                        routeString, 
                        controllerType.Name, 
                        methodQueryItem.MethodInfo.Name), rpe);

                    continue;
                }

                this.controllerRegistrations.Add(new ControllerRegistration
                    {
                        ControllerMethod = methodQueryItem.MethodInfo,
                        ControllerType = controllerType,
                        Route = route,
                        RouteTemplate = routeString
                    });

                log.DebugFormat("Route \"{0}\" registered for controller method {1}.{2}.", routeString, controllerType.Name, methodQueryItem.MethodInfo.Name);
            }
        }
    }
}
