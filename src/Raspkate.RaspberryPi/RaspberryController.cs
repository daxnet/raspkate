using Raspkate.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.RaspberryPi
{
    [RoutePrefix("raspberry")]
    public class RaspberryController : RaspkateController
    {
        [HttpGet]
        [Route("board")]
        public dynamic GetBoardInformation()
        {
            return new
            {
                Raspberry.Board.Current.IsRaspberryPi,
                Firmware = Raspberry.Board.Current.Firmware.ToString(),
                Model = Enum.GetName(typeof(Raspberry.Model), Raspberry.Board.Current.Model)
            };
        }
    }
}
