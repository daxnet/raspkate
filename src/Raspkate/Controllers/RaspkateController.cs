using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    /// <summary>
    /// Represents the base class of Raspkate controllers.
    /// </summary>
    public abstract class RaspkateController
    {
        /// <summary>
        /// The internal object used for locking and synchronzation.
        /// </summary>
        internal readonly object _syncObject = new object();
    }
}
