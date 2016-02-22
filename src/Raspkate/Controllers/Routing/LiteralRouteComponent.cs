using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    internal sealed class LiteralRouteComponent : RouteComponent
    {
        protected override string MatchingExpression
        {
            get { return @"^(?<name>)\w+$"; }
        }
    }
}
