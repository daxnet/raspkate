using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    /// <summary>
    /// Represents a particular component is a route string.
    /// </summary>
    internal abstract class RouteComponent
    {
        public const string NameGroup = "name";

        public virtual bool IsMatch(string src)
        {
            var regex = new Regex(this.MatchingExpression);
            var match = regex.Match(src);
            if (match.Success && match.Groups[NameGroup] != null)
            {
                this.Name = match.Groups[NameGroup].Value;
                if (!string.IsNullOrEmpty(this.Name))
                {
                    return true;
                }
            }
            return false;
        }

        public string Name { get; protected set; }

        protected abstract string MatchingExpression { get; }
    }
}
