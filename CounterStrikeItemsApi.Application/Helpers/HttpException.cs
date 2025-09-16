using System.Net;

namespace CounterStrikeItemsApi.Application.Helpers
{
    public class HttpException(HttpStatusCode statusCode, string message) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;

        public HttpException(int statusCode, string message): this((HttpStatusCode)statusCode, message) { }
    }
}
