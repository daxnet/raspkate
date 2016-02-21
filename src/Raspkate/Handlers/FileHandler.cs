using System.Linq;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;

namespace Raspkate.Handlers
{
    internal sealed class FileHandler : RaspkateHandler
    {
        internal const string Pattern =
            @"^(?<fileName>/?(\w(\w|\-|\s)*/)*(\w|\-)+(\.(\w|\-)+)*\.\w+)(\?(?<queryString>\w+=(\w|\-)+(&\w+=(\w|\-)+)*))?$";

        private readonly Regex regularExpression = new Regex(Pattern);
        private readonly ThreadLocal<string> requestedFileName = new ThreadLocal<string>(() => string.Empty);

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public FileHandler(RaspkateServer server, IEnumerable<KeyValuePair<string, string>> properties)
            : base(server, properties)
        {
        }


        public override bool ShouldHandle(HttpListenerRequest request)
        {
            if (request.RawUrl == "/")
            {
                var defaultPages = this.GetPropertyValue("DefaultPages");
                if (!string.IsNullOrEmpty(defaultPages))
                {
                    var defaultPageList = defaultPages.Split(';').Select(p => p.Trim());
                    foreach(var defaultPage in defaultPageList)
                    {
                        var fullName = Path.Combine(this.BasePath, defaultPage);
                        if (File.Exists(fullName))
                        {
                            this.requestedFileName.Value = defaultPage;
                            return true;
                        }
                    }
                }
                return false;
            }

            var match = this.regularExpression.Match(request.RawUrl.Trim('\\', '/', '?'));
            if (match.Success)
            {
                this.requestedFileName.Value = match.Groups["fileName"].Value;
                return true;
            }
            return false;
        }

        public override void Process(HttpListenerRequest request, HttpListenerResponse response)
        {
            var fileNameRequested = Path.Combine(this.BasePath, this.requestedFileName.Value.Replace("/", "\\"));
            log.DebugFormat("File requested: {0}", fileNameRequested);
            if (!File.Exists(fileNameRequested))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                var fileBytes = File.ReadAllBytes(fileNameRequested);
                response.StatusCode = 200;
                response.ContentType = Utils.GetMimeType(Path.GetExtension(fileNameRequested));
                response.ContentLength64 = fileBytes.LongLength;
                response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
            }
        }

        private string BasePath
        {
            get
            {
                if (this.Server.Configuration.Relative)
                {
                    return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), this.Server.Configuration.BasePath);
                }
                return this.Server.Configuration.BasePath;
            }
        }
    }
}
