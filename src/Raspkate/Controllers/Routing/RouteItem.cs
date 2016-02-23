using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    /// <summary>
    /// Represents a particular component is a route string.
    /// </summary>
    internal abstract class RouteItem
    {
        public const string NameGroup = "name";

        public abstract bool Prepare(string itemTemplate);

        public string Name { get; protected set; }

        protected RouteItemAttribute Attribute
        {
            get
            {
                return (RouteItemAttribute)this.GetType().GetCustomAttributes(typeof(RouteItemAttribute), false).FirstOrDefault();
            }
        }
    }
}
