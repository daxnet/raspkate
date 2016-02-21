using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Config
{
    partial class RaspkateConfiguration
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
            sb.Append("{ \"Prefix\": \"" + this.Prefix + "\", \"BasePath\": \"" + this.BasePath + "\", \"Relative\": \"" + this.Relative + "\", \"Handlers\": [");
            if (this.Handlers.Count > 0)
            {
                sb.Append(this.Handlers);
            }
            sb.Append("]}");
            return sb.ToString();
        }
    }
}
