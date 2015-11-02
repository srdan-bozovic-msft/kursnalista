using System;
using System.Collections.Generic;
using Windows.Web.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MSC.Universal.Shared.Contracts.Services
{
    public interface IHttpClientService
    {
        Task<string> GetRawAsync(string url, CancellationToken cancellationToken);
        Task<T> GetJsonAsync<T>(string url, CancellationToken cancellationToken);
        Task<T> GetXmlAsync<T>(string url, CancellationToken cancellationToken);

        Task<Tuple<HttpStatusCode, string>> CallAsync(string verb,
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            IEnumerable<KeyValuePair<string, string>> content,
            CancellationToken cancellationToken);
    }
}
