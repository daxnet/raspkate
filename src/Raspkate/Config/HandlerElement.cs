using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Config
{
    partial class HandlerElement
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
            sb.Append("{ \"Name\": \"" + this.Name + "\", \"Type\": \""+ this.Type + "\", \"Properties\": [");
            if (this.HandlerProperties.Count > 0)
            {
                sb.Append(this.HandlerProperties);
            }
            sb.Append("] }");
            return sb.ToString();
        }
    }
}
