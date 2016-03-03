using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Reflection;

namespace Raspkate
{
    public abstract class RaspkateHandler : IRaspkateHandler
    {
        private readonly RaspkateServer server;
        private readonly IEnumerable<KeyValuePair<string, string>> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        /// <param name="server">The <see cref="UnifiedSpaServer"/> instance on which the module
        /// is registered and executed.</param>
        protected RaspkateHandler(RaspkateServer server, IEnumerable<KeyValuePair<string, string>> properties)
        {
            this.server = server;
            this.properties = properties;
        }

        /// <summary>
        /// Gets the <see cref="UnifiedSpaServer"/> instance on which the module
        /// is registered and executed.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        protected RaspkateServer Server
        {
            get { return this.server; }
        }

        protected string GetPropertyValue(string propertyName)
        {
            return (from property in this.properties where property.Key == propertyName select property.Value).FirstOrDefault();
        }

        protected internal virtual void OnRegistering() { }

        protected internal virtual void OnUnregistered() { }

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
        public abstract void Process(HttpListenerRequest request, HttpListenerResponse response);
    }
}
