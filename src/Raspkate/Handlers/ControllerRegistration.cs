using Raspkate.Controllers.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Handlers
{
    internal sealed class ControllerRegistration
    {
        public string RouteTemplate { get; set; }
        public Route Route { get; set; }
        public Type ControllerType { get; set; }
        public MethodInfo ControllerMethod { get; set; }
    }
}
