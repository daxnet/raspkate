﻿using System;
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
            
        }

        public IEnumerable<RaspkateHandler> HttpHandlers
        {
            get { return this.httpHandlers; }
        }

        public RaspkateConfiguration Configuration
        {
            get { return this.configuration; }
        }

        public void Start()
        {
            log.Info(string.Format("Starting Raspkate service with the configuration:{0}{1}", Environment.NewLine, this.configuration));
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
            log.Info("Raspkate service stopped successfully.");
        }

        private void RegisterHandlers(RaspkateConfiguration config)
        {
            foreach(HandlerElement handlerElement in config.Handlers)
            {
                var handlerType = Type.GetType(handlerElement.Type);
                if (handlerType == null ||
                    !handlerType.IsSubclassOf(typeof(RaspkateHandler)))
                {
                    log.WarnFormat("Cannot load CLR type from name \"{0}\", skipping...", handlerElement.Type);
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
                if (handler != null)
                {
                    handler.OnRegistering();
                    this.httpHandlers.Add(handler);
                    log.InfoFormat("Handler \"{0}\" registered successfully.", handler);
                }
                else
                {
                    log.WarnFormat("Register handler \"{0}\" failed, skipping...", handlerElement.Type);
                }
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
            if (this.thread.IsAlive)
            {
                var httpListener = (HttpListener)result.AsyncState;
                IAsyncResult _result = httpListener.BeginGetContext(new AsyncCallback(OnGetContext), httpListener);
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
            var handled = false;
            foreach (var handler in this.httpHandlers)
            {
                if (handler.ShouldHandle(context.Request))
                {
                    handler.Process(context.Request, context.Response);
                    handled = true;
                    break;
                }
                else
                {
                    log.DebugFormat("Handler \"{0}\" cannot handle the request with URL \"{1}\", skipped.", handler, context.Request.Url);
                }
            }

            if (!handled)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
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
