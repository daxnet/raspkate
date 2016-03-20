using Raspberry.IO.GeneralPurpose;
using Raspkate.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Modules.RaspberryPi
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("raspberry")]
    [Synchronized]
    internal class RaspberryController : RaspkateController
    {
        private static readonly bool IsRaspberryPi = Raspberry.Board.Current.IsRaspberryPi;
        private static readonly PinDescription[] PinDescriptions = new[] { 
            new PinDescription("3.3v", PinType.Power), new PinDescription("5v", PinType.Power),
            new PinDescription("I2C1_SDA", PinType.I2C), new PinDescription("5v", PinType.Power),
            new PinDescription("I2C1_SCL", PinType.I2C), new PinDescription("GND", PinType.Ground),
            new PinDescription("GPIO_4", PinType.Gpio), new PinDescription("UART_TXD", PinType.UART),
            new PinDescription("GND", PinType.Ground), new PinDescription("UART_RXD", PinType.UART),
            new PinDescription("GPIO_17", PinType.Gpio), new PinDescription("GPIO_18", PinType.Gpio),
            new PinDescription("GPIO_27", PinType.Gpio), new PinDescription("GND", PinType.Ground),
            new PinDescription("GPIO_22", PinType.Gpio), new PinDescription("GPIO_23", PinType.Gpio),
            new PinDescription("3.3v", PinType.Power), new PinDescription("GPIO_24", PinType.Gpio),
            new PinDescription("SPI_MOSI", PinType.SPI), new PinDescription("GND", PinType.Ground),
            new PinDescription("SPI_MISO", PinType.SPI), new PinDescription("GPIO_25", PinType.Gpio),
            new PinDescription("SPI_SCLK", PinType.SPI), new PinDescription("SPI_CE0", PinType.SPI),
            new PinDescription("GND", PinType.Ground), new PinDescription("SPI_CE1", PinType.SPI),
            new PinDescription("ID_SD", PinType.ID_EEPROM), new PinDescription("ID_SC", PinType.ID_EEPROM),
            new PinDescription("GPIO_5", PinType.Gpio), new PinDescription("GND", PinType.Ground),
            new PinDescription("GPIO_6", PinType.Gpio), new PinDescription("GPIO_12", PinType.Gpio),
            new PinDescription("GPIO_13", PinType.Gpio), new PinDescription("GND", PinType.Ground),
            new PinDescription("GPIO_19", PinType.Gpio), new PinDescription("GPIO_16", PinType.Gpio),
            new PinDescription("GPIO_26", PinType.Gpio), new PinDescription("GPIO_20", PinType.Gpio),
            new PinDescription("GND", PinType.Ground), new PinDescription("GPIO_21", PinType.Gpio)
        };
        private static readonly IGpioConnectionDriver Driver = GpioConnectionSettings.DefaultDriver;

        /// <summary>
        /// Gets the Raspberry Pi board information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("board")]
        public dynamic GetBoardInformation()
        {
            return new
            {
                IsRaspberryPi = IsRaspberryPi,
                Raspberry.Board.Current.IsOverclocked,
                Firmware = IsRaspberryPi ? Raspberry.Board.Current.Firmware.ToString() : "N/A",
                Model = IsRaspberryPi ? Enum.GetName(typeof(Raspberry.Model), Raspberry.Board.Current.Model) : "N/A",
                Processor = IsRaspberryPi ? Raspberry.Board.Current.ProcessorName : "N/A",
                SerialNumber = IsRaspberryPi ? Raspberry.Board.Current.SerialNumber : "N/A"
            };
        }

        [HttpGet]
        [Route("pins")]
        public dynamic GetPinsStatus()
        {
            if (IsRaspberryPi)
            {
                var model = Raspberry.Board.Current.Model;
                var totalPins = 26;
                if (model == Raspberry.Model.APlus || 
                    model == Raspberry.Model.BPlus ||
                    model == Raspberry.Model.B2)
                {
                    totalPins = 40;
                }
                var pins = new List<PinDescription>();
                var pinNames = Enum.GetNames(typeof(ConnectorPin));
                for (var i = 0; i < totalPins; i++)
                {
                    var pinDescription = PinDescriptions[i];
                    pinDescription.Value = 0;
                    var pinName = string.Format("P1Pin{0:D2}", i + 1);
                    if (pinNames.Contains(pinName))
                    {
                        ConnectorPin connectorPin;
                        if (Enum.TryParse<ConnectorPin>(pinName, out connectorPin))
                        {
                            var processorPin = connectorPin.ToProcessor();
                            pinDescription.Value = Driver.Read(processorPin) ? 1 : 0;
                        }
                    }
                    pins.Add(pinDescription);
                }

                return new
                {
                    Model = Enum.GetName(typeof(Raspberry.Model), model),
                    Pins = pins
                };
            }
            return Error("The current device is not a Raspberry Pi...234234.");
        }
    }
}
