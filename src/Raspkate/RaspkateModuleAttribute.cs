using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate
{
    public enum ModuleConfigurationLocation
    {
        File,
        Resource
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public sealed class RaspkateModuleAttribute : Attribute
    {
        public ModuleConfigurationLocation Location { get; set; }
        public string Resource { get; set; }

        public RaspkateModuleAttribute(string resource)
            : this(resource, ModuleConfigurationLocation.File)
        { }

        public RaspkateModuleAttribute(string resource, ModuleConfigurationLocation location)
        {
            this.Resource = resource;
            this.Location = location;
        }
    }
}
