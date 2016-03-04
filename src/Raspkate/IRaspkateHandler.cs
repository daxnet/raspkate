using System.Net;

namespace Raspkate
{
    /// <summary>
    /// Represents that the implemented classes are Raspkate HTTP handlers.
    /// </summary>
    public interface IRaspkateHandler
    {
        /// <summary>
        /// Returns a <see cref="bool"/> value which indicates whether the current handler
        /// can handle the given HTTP request.
        /// </summary>
        /// <param name="request">The request object to be validated.</param>
        /// <returns><c>True</c> if current handler can handle the given request, otherwise, <c>False</c>.</returns>
        bool ShouldHandle(HttpListenerRequest request);

        /// <summary>
        /// Processes the given request and returns the response.
        /// </summary>
        /// <param name="request">The request to be processed by current handler.</param>
        /// <param name="response">The response which contains the returned data.</param>
        void Process(HttpListenerRequest request, HttpListenerResponse response);
    }
}
