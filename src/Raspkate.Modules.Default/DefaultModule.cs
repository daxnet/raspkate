using Raspkate.Config;
using Raspkate.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Modules.Default
{
    internal sealed class DefaultModule : RaspkateModule
    {
        public DefaultModule(ModuleContext context)
            : base(context)
        { }

        protected override IEnumerable<IRaspkateHandler> CreateHandlers()
        {
            var defaultPages = this.Context.ReadSetting("FileHandler.DefaultPages");
            if (string.IsNullOrEmpty(defaultPages))
            {
                throw new ConfigurationException("No DefaultPages specified.");
            }

            var fileHandlerBasePath = this.Context.ReadSetting("FileHandler.BasePath");
            if (string.IsNullOrEmpty(fileHandlerBasePath))
            {
                throw new ConfigurationException("No BasePath specified.");
            }

            var fileHandlerIsRelativePath = this.Context.ReadSetting("FileHandler.IsRelativePath");
            var isRelativePath = false;
            bool.TryParse(fileHandlerIsRelativePath, out isRelativePath);

            var basePath = fileHandlerBasePath;
            if (isRelativePath)
            {
                basePath = Path.Combine(Context.ModuleFolder, basePath);
            }

            yield return new FileHandler("Raspkate.Modules.DefaultModule.FileHandler", defaultPages, basePath);
            yield return new ControllerHandler("Raspkate.Modules.DefaultModule.ControllerHandler", new[] { typeof(DefaultRaspkateController) });
        }
    }
}
