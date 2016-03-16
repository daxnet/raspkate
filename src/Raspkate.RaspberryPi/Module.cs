using Raspkate.Handlers;
using Raspkate.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.RaspberryPi
{
    internal sealed class Module : RaspkateModule
    {
        private readonly IDictionary<string, Assembly> dependencies = new Dictionary<string, Assembly>();

        public Module(ModuleContext context)
            : base(context)
        {
            foreach(var file in Directory.EnumerateFiles(Context.ModuleFolder, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var dependency = Assembly.LoadFrom(file);
                    this.dependencies.Add(dependency.FullName, dependency);
                }
                catch { }
            }
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly resolution;
            dependencies.TryGetValue(args.Name, out resolution);
            return resolution;
        }

        protected override IEnumerable<IRaspkateHandler> CreateHandlers()
        {
            yield return new FileHandler("Raspkate.RaspberryPi.Module.FileHandler", "index.htm;index.html", Path.Combine(Context.ModuleFolder, "web"));
            yield return new ControllerHandler("Raspkate.RaspberryPi.Module.ControllerHandler", new[] { typeof(RaspberryController) });
        }
    }

}
