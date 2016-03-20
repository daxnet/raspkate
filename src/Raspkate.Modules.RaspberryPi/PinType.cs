using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Modules.RaspberryPi
{
    /// <summary>
    /// Represents the types of the pin.
    /// </summary>
    internal enum PinType
    {
        Power,
        Ground,
        Gpio,
        I2C,
        SPI,
        UART,
        ID_EEPROM
    }
}
