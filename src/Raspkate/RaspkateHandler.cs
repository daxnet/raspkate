using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Reflection;

namespace Raspkate
{
    public abstract class RaspkateHandler : IRaspkateHandler
    {
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        /// <param name="server">The <see cref="UnifiedSpaServer"/> instance on which the module
        /// is registered and executed.</param>
        protected RaspkateHandler(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets the name of the handler.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
        }

        public virtual void OnRegistering() { }

        public virtual void OnUnregistered() { }

        /// <summary>
        /// Checks if the incoming request can be processed by the current module.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        /// <returns><c>True</c> if the current module can process the incoming request, otherwise, <c>False</c>. </returns>
        public abstract bool ShouldHandle(HttpListenerRequest request);

        /// <summary>
        /// Processes the given request and populate the response with the processing result.
        /// </summary>
        /// <param name="request">The request which should be processed.</param>
        /// <param name="response">The response to which the processing result is populated.</param>
        public abstract HandlerProcessResult Process(HttpListenerRequest request);
    }
}
