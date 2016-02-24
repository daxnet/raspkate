using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    internal static class RouteParser
    {
        private static readonly List<Type> RegisteredRouteTypes = new List<Type> { typeof(LiteralRouteItem), typeof(ParameterRouteItem) };

        public static Route Parse(string routePattern)
        {
            var splittedItems = routePattern.Split('/');
            Route route = new Route();
            foreach (var splittedItem in splittedItems)
            {
                var routeItemType = RegisteredRouteTypes.FirstOrDefault(x => x.IsDefined(typeof(RouteItemAttribute), false) &&
                    ((RouteItemAttribute)(x.GetCustomAttributes(typeof(RouteItemAttribute), false).First())).MatchItemTemplate(splittedItem).Success);

                if (routeItemType == null)
                {
                    throw new RouteParseException("There is no registered route item that can handle the item template \"{0}\".", splittedItem);
                }

                var routeItem = (RouteItem)Activator.CreateInstance(routeItemType);
                if (!routeItem.Prepare(splittedItem))
                {
                    throw new RouteParseException("Failed to prepare the route item for item template \"{0}\", the route item might not be able to extract the required information.", splittedItem);
                }
                route.Add(routeItem);
            }
            return route;
        }
    }
}
