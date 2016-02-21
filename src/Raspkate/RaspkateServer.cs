using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Raspkate.Config;

namespace Raspkate
{
    public class RaspkateServer
    {
        private readonly RaspkateConfiguration configuration;
        private readonly HttpListener listener = new HttpListener();
        private Thread thread;
        private volatile bool cancelled;
        private volatile bool prefixesRegistered;
        private readonly ManualResetEvent stopEvent = new ManualResetEvent(false);
        private readonly List<RaspkateHandler> httpHandlers = new List<RaspkateHandler>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RaspkateServer(RaspkateConfiguration configuration)
        {
            this.configuration = configuration;
            this.RegisterPrefixes(new[] { configuration.Prefix });
            this.RegisterHandlers(configuration);
        }

        public IEnumerable<RaspkateHandler> HttpHandlers
        {
            get { return this.httpHandlers; }
        }

        public RaspkateConfiguration Configuration
        {
            get { return this.configuration; }
        }

        public void RegisterHandler<T>()
            where T : RaspkateHandler
        {
            this.httpHandlers.Add((RaspkateHandler) Activator.CreateInstance(typeof (T), this));
        }

        public void RegisterHandler(RaspkateHandler handler)
        {
            this.httpHandlers.Add(handler);
        }

        public void Start()
        {
            this.listener.Start();
            this.thread = new Thread(this.ExecuteThread);
            this.thread.Start(this.listener);
            log.Info("Service started.");
        }

        public void Stop()
        {
            this.cancelled = true;
            this.stopEvent.Set();
            this.thread.Join();
            this.listener.Stop();
        }

        private void RegisterHandlers(RaspkateConfiguration config)
        {
            foreach(HandlerElement handlerElement in config.Handlers)
            {
                var handlerType = Type.GetType(handlerElement.Type);
                if (handlerType == null ||
                    !handlerType.IsSubclassOf(typeof(RaspkateHandler)))
                {
                    // TODO: Write warn log
                    continue;
                }

                Dictionary<string, string> properties = new Dictionary<string, string>();
                if (handlerElement.HandlerProperties != null)
                {
                    foreach(HandlerPropertyElement hpe in handlerElement.HandlerProperties)
                    {
                        properties.Add(hpe.Name, hpe.Value);
                    }
                }

                var handler = (RaspkateHandler)Activator.CreateInstance(handlerType, this, properties);
                this.RegisterHandler(handler);
            }
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
            var httpListener = (HttpListener)result.AsyncState;
            var context = httpListener.EndGetContext(result);
            this.ProcessRequest(context);
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                var handled = false;
                foreach (var handler in this.httpHandlers)
                {
                    if (handler.ShouldHandle(context.Request))
                    {
                        handler.Process(context.Request, context.Response);
                        handled = true;
                        break;
                    }
                }

                if (!handled)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                this.WriteResponse(context.Response, ex);
            }
            finally
            {
                context.Response.Close();
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

        internal void WriteResponse(HttpListenerResponse response, HttpStatusCode statusCode, object body)
        {
            response.StatusCode = (int)statusCode;
            response.ContentEncoding = Encoding.UTF8;
            var bodyJson = JsonConvert.SerializeObject(body);
            var bytes = Encoding.UTF8.GetBytes(bodyJson);
            response.ContentLength64 = bytes.LongLength;
            response.ContentType = "application/json";
            response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        internal void WriteResponse(HttpListenerResponse response, Exception exception)
        {
            this.WriteResponse(response, HttpStatusCode.InternalServerError, exception);
        }
    }
}
