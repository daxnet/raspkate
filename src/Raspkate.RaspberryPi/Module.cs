using Raspkate.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.RaspberryPi
{
    internal sealed class Module : IRaspkateModule
    {
        private static readonly Lazy<IEnumerable<IRaspkateHandler>> registeredHandlers = new Lazy<IEnumerable<IRaspkateHandler>>(() => new List<IRaspkateHandler> { 
                    new FileHandler("RaspberryPiStaticFileHandler", new Dictionary<string, string> { 
                        { "DefaultPages", "index.htm;index.html" },
                        { "BasePath", @"prov\RaspberryPi\web" },
                        { "IsRelativePath", "true" }
                    }),
                    new ControllerHandler("RaspberryPiWebAPIHandler", new [] { typeof(RaspberryController) } )
                });

        public IEnumerable<IRaspkateHandler> RegisteredHandlers
        {
            get
            {
                return registeredHandlers.Value;
            }
        }
    }
}
