using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Modules
{
    public sealed class ModuleContext
    {
        private const string SettingsFileName = "settings.json";
        private readonly Lazy<IEnumerable<KeyValuePair<string, string>>> settings;

        internal ModuleContext(string rootFolder, string moduleFolder)
        {
            this.RootFolder = rootFolder;
            this.ModuleFolder = moduleFolder;
            settings = new Lazy<IEnumerable<KeyValuePair<string, string>>>(() =>
            {
                var result = new Dictionary<string, string>();
                if (File.Exists(SettingsFile))
                {
                    dynamic configuration = JsonConvert.DeserializeObject(File.ReadAllText(SettingsFile));
                    foreach (dynamic setting in configuration.settings)
                    {
                        result.Add((string)setting.key, (string)setting.value);
                    }
                }
                return result;
            });
        }

        public string RootFolder { get; internal set; }

        public string ModuleFolder { get; internal set; }

        public string SettingsFile
        {
            get { return Path.Combine(this.ModuleFolder, SettingsFileName); }
        }

        public IEnumerable<KeyValuePair<string, string>> Settings
        {
            get
            {
                return settings.Value;
            }
        }

        public string ReadSetting(string key)
        {
            var setting = this.Settings.FirstOrDefault(x => x.Key == key);
            return setting.Value;
        }
    }
}
