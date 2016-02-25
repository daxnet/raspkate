using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    [RoutePrefix("api")]
    public class DefaultRaspkateController : RaspkateController
    {
        public DefaultRaspkateController()
        { }

        [HttpGet]
        [Route("server/info")]
        public dynamic GetServerInfo()
        {
            return new
            {
                Environment.MachineName,
                Environment.Is64BitOperatingSystem,
                Environment.OSVersion.Platform,
                Environment.OSVersion.VersionString,
                Environment.OSVersion.ServicePack,
                Environment.ProcessorCount,
                Environment.SystemDirectory,
                Environment.SystemPageSize,
                Version = Environment.Version.ToString(),
                Environment.CurrentManagedThreadId
            };
        }
    }
}
