using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    /// <summary>
    /// Represents an HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited=true)]
    public abstract class HttpMethodAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HttpMethodAttribute"/> class.
        /// </summary>
        /// <param name="methodName">Name of the HTTP method in uppercase, e.g. GET, POST, PUT, etc.</param>
        protected HttpMethodAttribute(string methodName)
        {
            this.MethodName = methodName;
        }

        /// <summary>
        /// Gets the name of current HTTP method, in uppercase, e.g. GET, POST, PUT, etc.
        /// </summary>
        public string MethodName { get; private set; }
    }
}
