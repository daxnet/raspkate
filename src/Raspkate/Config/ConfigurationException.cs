using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Config
{
    public class ConfigurationException : RaspkateException
    {
        public ConfigurationException()
        { }

        public ConfigurationException(string message)
            : base(message)
        { }

        public ConfigurationException(string format, params string[] args)
            : base(string.Format(format, args))
        { }

        public ConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
