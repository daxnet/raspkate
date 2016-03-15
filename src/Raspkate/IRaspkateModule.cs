using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate
{
    public interface IRaspkateModule
    {
        IEnumerable<IRaspkateHandler> RegisteredHandlers { get; }
    }
}
