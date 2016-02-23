using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    [RouteItem(@"^{(?<name>\w+)(:(?<type>\w+)(:(?<constraint>.+))?)?}$")]
    internal sealed class ParameterRouteItem : RouteItem
    {
        public const string TypeGroup = "type";
        public const string ConstraintGroup = "constraint";

        public override bool Prepare(string itemTemplate)
        {
            var match = this.Attribute.MatchItemTemplate(itemTemplate);
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
                else
                {
                    this.ParameterType = typeof(object);
                }

                return true;
            }
            return false;
        }

        public object GetValue(string input)
        {
            try
            {
                if (this.ParameterType != null)
                    return Convert.ChangeType(input, this.ParameterType);
            }
            catch
            {
                
            }
            return null;
        }

        public Type ParameterType { get; private set; }

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
                default:
                    return typeof(string);
            }
        }
    }
}
