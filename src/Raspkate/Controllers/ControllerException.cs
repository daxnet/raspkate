using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    public class ControllerException : RaspkateException
    {
        public ControllerException()
        { }

        public ControllerException(string message)
            : base(message)
        { }

        public ControllerException(string format, params string[] args)
            : base(string.Format(format, args))
        { }

        public ControllerException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
