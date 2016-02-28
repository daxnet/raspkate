using Raspberry.IO.GeneralPurpose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    [RoutePrefix("api")]
    [Synchronized]
    public class DefaultRaspkateController : RaspkateController
    {
        private readonly IGpioConnectionDriver driver;
        private readonly bool isRaspberryPi = Raspberry.Board.Current.IsRaspberryPi;

        public DefaultRaspkateController()
        {
            if (isRaspberryPi)
            {
                driver = GpioConnectionSettings.DefaultDriver;
            }
        }

        [HttpGet]
        [Route("server/info")]
        public dynamic GetServerInfo()
        {
            return new
            {
                Environment.MachineName,
                Environment.Is64BitOperatingSystem,
                OSVersion = Environment.OSVersion.VersionString,
                OSPlatform = Enum.GetName(typeof(PlatformID), Environment.OSVersion.Platform),
                OSServicePack = Environment.OSVersion.ServicePack,
                Environment.ProcessorCount,
                Environment.SystemDirectory,
                Environment.SystemPageSize,
                FrameworkVersion = Environment.Version.ToString(),
                IsRaspberryPiDevice = isRaspberryPi,
                RaspberryPiModel = isRaspberryPi ? Raspberry.Board.Current.Model.ToString() : "N/A",
                RaspberryPiProcessorName = isRaspberryPi ? Raspberry.Board.Current.ProcessorName : "N/A",
                RaspberryPiSerialNumber = isRaspberryPi ? Raspberry.Board.Current.SerialNumber : "N/A"
            };
        }

        [HttpPost]
        [Route("setPin/{pin}/{value}")]
        public void SetPinValue(int pin, bool value)
        {
            if (isRaspberryPi)
            {
                var p = ((ConnectorPin)pin).ToProcessor();
                var driver = GpioConnectionSettings.DefaultDriver;
                driver.Allocate(p, PinDirection.Output);
                driver.Write(p, value);
            }
        }

        [HttpGet]
        [Route("getPin/{pin}")]
        public bool GetPinValue(int pin)
        {
            if (isRaspberryPi)
            {
                var p = ((ConnectorPin)pin).ToProcessor();
                var driver = GpioConnectionSettings.DefaultDriver;
                return driver.Read(p);
            }
            return false;
        }
    }
}
