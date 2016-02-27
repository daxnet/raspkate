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
                FrameworkVersion = Environment.Version.ToString(),
                IsRaspberryPiDevice = Raspberry.Board.Current.IsRaspberryPi,
                RaspberryPiModel = Raspberry.Board.Current.IsRaspberryPi ? Raspberry.Board.Current.Model.ToString() : "N/A",
                RaspberryPiProcessorName = Raspberry.Board.Current.IsRaspberryPi ? Raspberry.Board.Current.ProcessorName : "N/A",
                RaspberryPiSerialNumber = Raspberry.Board.Current.IsRaspberryPi ? Raspberry.Board.Current.SerialNumber : "N/A"
            };
        }

    }
}
