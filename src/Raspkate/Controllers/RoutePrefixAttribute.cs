using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RoutePrefixAttribute : Attribute
    {
        public RoutePrefixAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
