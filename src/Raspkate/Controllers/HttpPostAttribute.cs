using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    /// <summary>
    /// Represents the HTTP POST method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instace of <see cref="HttpPostAttribute"/> class.
        /// </summary>
        public HttpPostAttribute()
            : base("POST")
        { }
    }
}
