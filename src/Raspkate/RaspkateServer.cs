using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Raspkate.Config;
using System.Reflection;
using Raspkate.Properties;
using System.IO;
using System.Xml.Serialization;
using Raspkate.Modules;

namespace Raspkate
{
    public class RaspkateServer : IRaspkateServer
    {
        private Thread thread;
        private readonly RaspkateConfiguration configuration;
        private readonly HttpListener listener = new HttpListener();
        private volatile bool cancelled;
        private volatile bool prefixesRegistered;
        private readonly ManualResetEvent stopEvent = new ManualResetEvent(false);
        private readonly List<IRaspkateHandler> handlers = new List<IRaspkateHandler>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RaspkateServer(RaspkateConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<IRaspkateHandler> Handlers
        {
            get { return this.handlers; }
        }

        public RaspkateConfiguration Configuration
        {
            get { return this.configuration; }
        }

        public void Start()
        {
            log.Info(Resources.Logo);
            log.Info("Raspkate - A small and lightweight Web Server");
            log.InfoFormat("[ Version: {0} ]", Version);
            log.DebugFormat("Starting Raspkate service with the configuration:{0}{1}", Environment.NewLine, this.configuration);
            this.RegisterPrefixes(new[] { configuration.Prefix });
            this.RegisterHandlers(configuration);
            log.Debug("Starting HttpListener based on configuration.");
            this.listener.Start();
            log.Debug("HttpListener started successfully.");
            log.Debug("Starting service thread.");
            this.thread = new Thread(this.ExecuteThread);
            this.thread.Start(this.listener);
            log.DebugFormat("Service thread started successfully, ManagedThreadId={0}", this.thread.ManagedThreadId);
            log.Info("Service started.");
        }

        public void Stop()
        {
            log.Debug("Sending the STOP signal.");
            this.cancelled = true;
            this.stopEvent.Set();
            log.Debug("Waiting for service thread.");
            this.thread.Join();
            log.Debug("Service thread stopped successfully.");
            log.Debug("Stopping HttpListener.");
            this.listener.Stop();
            log.Debug("HttpListener stopped successfully.");
            this.UnregisterHandlers();
            log.Info("Raspkate service stopped successfully.");
        }

        private static Version Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        private void RegisterHandlers(RaspkateConfiguration config)
        {
            if (config.Modules != null && config.Modules.Count > 0)
            {
                var entryAssemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                foreach(ModuleElement moduleElement in config.Modules)
                {
                    var searchPath = moduleElement.IsRelative ?
                        Path.Combine(entryAssemblyPath, moduleElement.Path) :
                        moduleElement.Path;
                    if (Directory.Exists(searchPath))
                    {
                        foreach (var moduleAssemblyFile in Directory.EnumerateFiles(searchPath, "*.dll", SearchOption.AllDirectories))
                        {
                            try
                            {
                                var moduleAssembly = Assembly.LoadFile(moduleAssemblyFile);
                                if (moduleAssembly != null)
                                {
                                    var moduleTypes = from type in moduleAssembly.GetTypes()
                                                      where typeof(IRaspkateModule).IsAssignableFrom(type)
                                                      select type;
                                    foreach(var moduleType in moduleTypes)
                                    {
                                        var context = new ModuleContext(entryAssemblyPath, Path.GetDirectoryName(moduleAssemblyFile));
                                        var module = (IRaspkateModule)Activator.CreateInstance(moduleType, context);
                                        if (module != null && module.RegisteredHandlers != null)
                                        {
                                            foreach (var handler in module.RegisteredHandlers)
                                            {
                                                handler.OnRegistering();
                                                this.handlers.Add(handler);
                                                log.InfoFormat("Handler \"{0}\" registered successfully.", handler.Name);
                                            }
                                        }
                                        else
                                        {
                                            log.WarnFormat("No registered handlers being provided by type \"{0}\", skipping...", moduleType);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Debug(string.Format("Unable to load assembly from file \"{0}\".", moduleAssemblyFile), ex);
                            }
                        }
                    }
                    else
                    {
                        log.WarnFormat("Specified module path \"{0}\" does not exist.", searchPath);
                    }
                }
            }
        }

        private void UnregisterHandlers()
        {
            handlers.ForEach(h => h.OnUnregistered());
        }

        private void ExecuteThread(object arg)
        {
            var httpListener = (HttpListener)arg;
            while (!this.cancelled && httpListener.IsListening)
            {
                var asyncResult = httpListener.BeginGetContext(this.OnGetContext, httpListener);
                if (WaitHandle.WaitAny(new[] { this.stopEvent, asyncResult.AsyncWaitHandle }) == 0)
                {
                    break;
                }
            }
        }

        private void OnGetContext(IAsyncResult result)
        {
            if (this.thread.IsAlive)
            {
                var httpListener = (HttpListener)result.AsyncState;
                httpListener.BeginGetContext(new AsyncCallback(OnGetContext), httpListener);
                var context = httpListener.EndGetContext(result);
                try
                {
                    this.ProcessRequest(context);
                }
                catch(HttpListenerException ex)
                {
                    log.DebugFormat("HttpListener raised exception. Written to log for debugging reference.", ex);
                }
                catch(Exception ex)
                {
                    log.Error("Exception occurred.", ex);
                }
                finally
                {
                    try
                    {
                        context.Response.OutputStream.Flush();
                        context.Response.OutputStream.Close();
                        context.Response.Close();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Cannot flush and close the response stream.", ex);
                    }
                    finally
                    { }
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var incorrectResults = new List<Tuple<IRaspkateHandler, HandlerProcessResult>>();
            foreach (var handler in this.handlers)
            {
                try
                {
                    if (handler.ShouldHandle(context.Request))
                    {
                        var result = handler.Process(context.Request);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            context.Response.WriteResponse(result);
                            return;
                        }
                        else
                        {
                            incorrectResults.Add(new Tuple<IRaspkateHandler, HandlerProcessResult>(handler, result));
                        }
                    }
                    else
                    {
                        log.DebugFormat("Handler \"{0}\" cannot handle the request with URL \"{1}\", skipped.", handler, context.Request.Url);
                    }
                }
                catch (Exception ex)
                {
                    log.Warn(string.Format("Failed to check whether the current handler (Handler: \"{0}\") can handle the given request, skipping...", handler), ex);
                }
            }

            if (incorrectResults.Count == 0)
            {
                context.Response.WriteResponse(HandlerProcessResult.Text(HttpStatusCode.BadRequest, "Invalid request."));
            }
            else if (incorrectResults.Count == 1)
            {
                context.Response.WriteResponse(incorrectResults[0].Item2);
            }
            else
            {
                var groupings = incorrectResults.GroupBy(x => x.Item2.StatusCode);
                if (groupings.Count() == 1)
                {
                    context.Response.WriteResponse(groupings.First().Key, incorrectResults);
                }
                else
                {
                    context.Response.WriteResponse(HttpStatusCode.InternalServerError, incorrectResults);
                }
            }
        }

        private void RegisterPrefixes(string[] prefixes)
        {
            if (!this.prefixesRegistered)
            {
                foreach (var prefix in prefixes)
                {
                    this.listener.Prefixes.Add(prefix);
                }
                this.prefixesRegistered = true;
            }
        }
    }
}
