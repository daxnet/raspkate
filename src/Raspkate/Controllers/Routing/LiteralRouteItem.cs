using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    [RouteItem(@"^(?<name>\w+)$")]
    internal sealed class LiteralRouteItem : RouteItem
    {
        public override bool Prepare(string itemTemplate)
        {
            var match = this.Attribute.MatchItemTemplate(itemTemplate);
            if (match.Success && match.Groups[NameGroup] != null)
            {
                this.Name = match.Groups[NameGroup].Value;
                if (!string.IsNullOrEmpty(this.Name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
