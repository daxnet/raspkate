using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    public class DefaultRaspkateController : RaspkateController
    {
        public DefaultRaspkateController()
        { }

        [HttpGet]
        [Route("services/machinename")]
        public string GetMachineName()
        {
            return Environment.MachineName;
        }
    }
}
