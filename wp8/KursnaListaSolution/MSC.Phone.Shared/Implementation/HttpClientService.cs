using MSC.Phone.Shared.Contracts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSC.Phone.Shared
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<T> GetJsonAsync<T>(string url, CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            var client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (response != null && (
                response.StatusCode == System.Net.HttpStatusCode.OK))
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            else
                return default(T);
        }
    }
}
