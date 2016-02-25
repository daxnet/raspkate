using Raspkate.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Handlers
{
    /// <summary>
    /// Represents the registration of a controller information.
    /// </summary>
    internal sealed class ControllerRegistration
    {
        /// <summary>
        /// Gets or sets the route template.
        /// </summary>
        public string RouteTemplate { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="Route"/> which represents a route,
        /// and can extract the values from that.
        /// </summary>
        public Route Route { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the controller.
        /// </summary>
        public Type ControllerType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MethodInfo"/> which represents a method
        /// in the controller that corresponds to the route.
        /// </summary>
        public MethodInfo ControllerMethod { get; set; }
    }
}
