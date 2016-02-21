using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Config
{
    partial class HandlerElementCollection
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            var i = 0;
            foreach(HandlerElement elem in this)
            {
                sb.Append(elem);
                if (i<this.Count-1)
                {
                    sb.Append(", ");
                }
                i++;
            }
            return sb.ToString();
        }
    }
}
