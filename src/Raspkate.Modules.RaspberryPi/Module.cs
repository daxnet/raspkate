using Raspkate.Handlers;
using Raspkate.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Modules.RaspberryPi
{
    internal sealed class Module : RaspkateModule
    {
        public Module(ModuleContext context)
            : base(context)
        {
            
        }

        protected override IEnumerable<IRaspkateHandler> CreateHandlers()
        {
            yield return new FileHandler("Raspkate.RaspberryPi.Module.FileHandler", "index.htm;index.html", Path.Combine(Context.ModuleFolder, "web"));
            yield return new ControllerHandler("Raspkate.RaspberryPi.Module.ControllerHandler", new[] { typeof(RaspberryController) });
        }
    }

}
