using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate
{
    public class RaspkateException : Exception
    {
        public RaspkateException()
        { }

        public RaspkateException(string message)
            : base(message)
        { }

        public RaspkateException(string format, params string[] args)
            : base(string.Format(format, args))
        { }

        public RaspkateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
