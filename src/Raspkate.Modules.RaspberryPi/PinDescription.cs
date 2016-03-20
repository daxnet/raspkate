using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Modules.RaspberryPi
{
    internal class PinDescription
    {
        public string Name { get; private set; }

        public PinType Type { get; private set; }

        public int Value { get; set; }

        public PinDescription(string name, PinType type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
