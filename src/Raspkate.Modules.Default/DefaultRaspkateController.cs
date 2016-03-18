using Raspkate.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Modules.Default
{
    [RoutePrefix("api")]
    [Synchronized]
    public class DefaultRaspkateController : RaspkateController
    {
        public DefaultRaspkateController()
        {
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
                FrameworkVersion = Environment.Version.ToString()
            };
        }

        //[HttpPost]
        //[Route("setPin/{pin}/{value}")]
        //public void SetPinValue(int pin, bool value)
        //{
        //    if (isRaspberryPi)
        //    {
        //        var p = ((ConnectorPin)pin).ToProcessor();
        //        var driver = GpioConnectionSettings.DefaultDriver;
        //        driver.Allocate(p, PinDirection.Output);
        //        driver.Write(p, value);
        //    }
        //}

        //[HttpGet]
        //[Route("getPin/{pin}")]
        //public bool GetPinValue(int pin)
        //{
        //    if (isRaspberryPi)
        //    {
        //        var p = ((ConnectorPin)pin).ToProcessor();
        //        var driver = GpioConnectionSettings.DefaultDriver;
        //        return driver.Read(p);
        //    }
        //    return false;
        //}
    }
}
