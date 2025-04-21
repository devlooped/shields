using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;

namespace Shields
{
    public static class CacheResponseExtensions
    {
        public static async Task<HttpResponseData> CreateCachedResponseDataAsync<T>(this HttpRequestData request, HttpStatusCode statusCode, T value, TimeSpan? maxAge = null)
        {
            var response = request.CreateResponse(statusCode);
            await response.WriteAsJsonAsync(value);

            var hostName = request.Url.Host;
            if (hostName != "localhost")
            {
                var cacheValue = $"public, max-age={maxAge?.TotalSeconds ?? 60}";
                response.Headers.Add("Cache-Control", cacheValue);
            }

            return response;
        }
    }
}
