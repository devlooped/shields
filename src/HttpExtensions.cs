using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Shields
{
    public static class CacheResponseExtensions
    {
        public static HttpResponseMessage CreateCachedResponse<T>(this HttpRequestMessage request, HttpStatusCode statusCode, T value, TimeSpan? maxAge = null)
        {
            var response = request.CreateResponse(statusCode);
            response.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            if (request.RequestUri.Host != "localhost")
            {
                response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = maxAge ?? TimeSpan.FromMinutes(1)
                };
            }

            return response;
        }
    }
}
