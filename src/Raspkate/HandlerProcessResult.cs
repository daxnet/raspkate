using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate
{
    /// <summary>
    /// Represents the processing result of a <see cref="RaspkateHandler"/>.
    /// </summary>
    public sealed class HandlerProcessResult
    {

        public static readonly HandlerProcessResult Success = new HandlerProcessResult(HttpStatusCode.OK, null, null);

        public static readonly Encoding ContentEncoding = Encoding.UTF8;

        public HttpStatusCode StatusCode { get; private set; }

        [JsonIgnore]
        public byte[] Content { get; private set; }

        public string ContentBase64
        {
            get
            {
                if (this.Content == null)
                {
                    return null;
                }
                return Convert.ToBase64String(this.Content);
            }
        }

        public string ContentType { get; private set; }

        private HandlerProcessResult(HttpStatusCode statusCode, string contentType, byte[] content)
        {
            this.StatusCode = statusCode;
            this.Content = content;
            this.ContentType = contentType;
        }

        public static HandlerProcessResult Text(HttpStatusCode statusCode, string content)
        {
            var contentBytes = ContentEncoding.GetBytes(content);
            return new HandlerProcessResult(statusCode, "text/plain", contentBytes);
        }

        public static HandlerProcessResult Json(HttpStatusCode statusCode, string jsonContent)
        {
            var contentBytes = ContentEncoding.GetBytes(jsonContent);
            return new HandlerProcessResult(statusCode, "application/json", contentBytes);
        }

        public static HandlerProcessResult File(HttpStatusCode statusCode, string contentType, byte[] content)
        {
            return new HandlerProcessResult(statusCode, contentType, content);
        }

        public static HandlerProcessResult Exception(HttpStatusCode statusCode, Exception ex)
        {
            return Text(statusCode, ex.ToString());
        }
    }
}
