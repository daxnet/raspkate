using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    /// <summary>
    /// Represents the HTTP GET method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpGetAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instancce of <see cref="HttpGetAttribute"/> class.
        /// </summary>
        public HttpGetAttribute()
            : base("GET")
        { }
    }
}
