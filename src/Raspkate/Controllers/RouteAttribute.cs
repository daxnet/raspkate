using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RouteAttribute : Attribute
    {
        public RouteAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
