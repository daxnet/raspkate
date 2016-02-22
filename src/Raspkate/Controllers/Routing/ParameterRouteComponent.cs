using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    internal sealed class ParameterRouteComponent : RouteComponent
    {
        public const string TypeGroup = "type";
        public const string ConstraintGroup = "constraint";

        public override bool IsMatch(string src)
        {
            var regex = new Regex(this.MatchingExpression);
            var match = regex.Match(src);
            if (match.Success && match.Groups[NameGroup] != null)
            {
                this.Name = match.Groups[NameGroup].Value;
                if (string.IsNullOrEmpty(this.Name))
                {
                    return false;
                }
                var parameterTypeName = match.Groups[TypeGroup] == null ? null : match.Groups[TypeGroup].Value;
                if (!string.IsNullOrEmpty(parameterTypeName))
                {
                    this.ParameterType = NameToType(parameterTypeName);
                }

            }
            return false;
        }

        public Type ParameterType { get; private set; }

        protected override string MatchingExpression
        {
            get { return @"^{(?<name>\w+)(:(?<type>\w+)(:(?<constraint>.+))?)?}$"; }
        }

        private static Type NameToType(string name)
        {
            var cname = name.ToLower();
            switch(cname)
            {
                case "int":
                    return typeof(int);
                case "bool":
                    return typeof(bool);
                case "datetime":
                    return typeof(DateTime);
                case "decimal":
                    return typeof(decimal);
                case "double":
                    return typeof(double);
                case "float":
                    return typeof(float);
                case "guid":
                    return typeof(Guid);
                case "string":
                    return typeof(string);
                default:
                    return null;
            }
        }
    }
}
