using Raspkate.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Modules
{
    public abstract class RaspkateModule : IRaspkateModule
    {
        private readonly ModuleContext context;
        private readonly Lazy<IEnumerable<IRaspkateHandler>> registeredHandlers;

        protected RaspkateModule(ModuleContext context)
        {
            this.context = context;
            this.registeredHandlers = new Lazy<IEnumerable<IRaspkateHandler>>(CreateHandlers);
            this.RegisterExternalDependencies();
        }

        protected ModuleContext Context { get { return this.context; } }

        protected abstract IEnumerable<IRaspkateHandler> CreateHandlers();

        protected virtual void RegisterExternalDependencies()
        {
            var externalDependencies = new Dictionary<string, Assembly>();
            foreach (var file in Directory.EnumerateFiles(Context.ModuleFolder, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var dependency = Assembly.LoadFrom(file);
                    if (dependency.FullName != Assembly.GetExecutingAssembly().FullName)
                    {
                        externalDependencies.Add(dependency.FullName, dependency);
                    }
                }
                catch { }
            }

            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
                {
                    Assembly resolution;
                    externalDependencies.TryGetValue(e.Name, out resolution);
                    return resolution;
                };
        }

        public IEnumerable<IRaspkateHandler> RegisteredHandlers
        {
            get { return registeredHandlers.Value; }
        }
    }
}
