using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RoutePrefixAttribute : Attribute
    {
        public RoutePrefixAttribute(string prefix)
        {
            this.Prefix = prefix;
        }

        public string Prefix { get; private set; }
    }
}
