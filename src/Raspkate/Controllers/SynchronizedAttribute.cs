using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SynchronizedAttribute : Attribute
    {
    }
}
