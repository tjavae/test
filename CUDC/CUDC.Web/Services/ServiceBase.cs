using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public abstract class ServiceBase
    {
        private readonly IConfiguration _config;

        protected ServiceBase(IConfiguration iconfig)
        {
            _config = iconfig;
        }

        protected async Task<T> GetAsync<T>(string requestUri)
        {
            var client = CreateHttpClient(requestUri);
            var response = await client.GetAsync(requestUri);
            var result = await ProcessResponseAsync<T>(response);
            return result;
        }

        protected async Task<T> PostAsync<T>(string requestUri, object data)
        {
            var payload = JsonConvert.SerializeObject(data);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var client = CreateHttpClient(requestUri);
            var response = await client.PostAsync(requestUri, content);
            if (response.Content.Headers.ContentLength == 0)
            {
                return default;
            }
            var result = await ProcessResponseAsync<T>(response);            
            return result;
        }

        private HttpClient CreateHttpClient(string requestUri)
        {
            if (requestUri != null && requestUri.Contains(_config["MisWebApiBaseUrl"]))
            {
                var clientHandler = new HttpClientHandler
                {
                    UseDefaultCredentials = true,
#if DEBUG
                    //Disable SSL certificate validation during local debugging only
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
#endif
                };

                return new HttpClient(clientHandler);
            }

            return new HttpClient();
        }

        private static async Task<T> ProcessResponseAsync<T>(HttpResponseMessage response)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(apiResponse);
            }

            if (string.IsNullOrEmpty(apiResponse))
            {
                return default;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)apiResponse;
            }
            
            return JsonConvert.DeserializeObject<T>(apiResponse); 
        }
    }
}
