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
            foreach(var controllerTypeName in controllerTypeNames)
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
            response.StatusCode = 200;
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            var bytes = Encoding.UTF8.GetBytes("<h1>Controller Handled</h1>");
            response.ContentLength64 = bytes.Length;
            response.OutputStream.Write(bytes, 0, bytes.Length);
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
            foreach(var methodQueryItem in methodQuery)
            {
                string routeString = string.Empty;
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
                var route = RouteParser.Parse(routeString);
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
