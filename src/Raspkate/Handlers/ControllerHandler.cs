using Raspkate.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspkate.Handlers
{
    internal sealed class ControllerHandler : RaspkateHandler
    {
        private readonly Regex fileNameRegularExpression = new Regex(FileHandler.Pattern);
        private readonly List<Type> controllerTypes = new List<Type>();
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
                this.controllerTypes.Add(controllerType);
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
    }
}
