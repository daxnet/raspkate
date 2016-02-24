using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    public class RouteParseException : RaspkateException
    {
        public RouteParseException()
        { }

        public RouteParseException(string message)
            : base(message)
        { }

        public RouteParseException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        public RouteParseException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
