using Raspkate.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate
{
    public interface IRaspkateServer
    {
        RaspkateConfiguration Configuration { get; }
        IEnumerable<IRaspkateHandler> Handlers { get; }
        void Start();
        void Stop();
    }
}
