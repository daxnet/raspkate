using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RouteItemAttribute : Attribute
    {
        public string Template { get; private set; }

        public RouteItemAttribute(string template)
        {
            this.Template = template;
        }

        public Match MatchItemTemplate(string itemTemplate)
        {
            return new Regex(this.Template).Match(itemTemplate);
        }
    }
}
