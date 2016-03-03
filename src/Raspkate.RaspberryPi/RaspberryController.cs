using Raspkate.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.RaspberryPi
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("raspberry")]
    internal class RaspberryController : RaspkateController
    {
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
                Raspberry.Board.Current.IsRaspberryPi,
                Firmware = Raspberry.Board.Current.Firmware.ToString(),
                Model = Enum.GetName(typeof(Raspberry.Model), Raspberry.Board.Current.Model)
            };
        }
    }
}
