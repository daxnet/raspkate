using Raspkate.Config;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        protected ModuleContext Context { get { return this.context; } }

        protected abstract IEnumerable<IRaspkateHandler> CreateHandlers();

        public IEnumerable<IRaspkateHandler> RegisteredHandlers
        {
            get { return registeredHandlers.Value; }
        }
    }
}
