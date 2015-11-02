using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using MSC.Universal.Shared.Contracts.Services;
using Newtonsoft.Json;

namespace MSC.Universal.Shared.Implementation
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<string> GetRawAsync(string url, CancellationToken cancellationToken)
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            httpBaseProtocolFilter.AutomaticDecompression = true;
            var client = new HttpClient(httpBaseProtocolFilter);
            var response = await client.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute)).AsTask(cancellationToken).ConfigureAwait(false);
            if (response != null && (
                response.IsSuccessStatusCode))
                return await response.Content.ReadAsStringAsync().AsTask(cancellationToken).ConfigureAwait(false);
            return null;
        }

        public async Task<T> GetJsonAsync<T>(string url, CancellationToken cancellationToken)
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            httpBaseProtocolFilter.AutomaticDecompression = true;
            var client = new HttpClient(httpBaseProtocolFilter);
            var response = await client.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute)).AsTask(cancellationToken).ConfigureAwait(false);
            if (response != null && (
                response.IsSuccessStatusCode))
            {
                var responseContent = await response.Content.ReadAsStringAsync().AsTask(cancellationToken).ConfigureAwait(false);
                try
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
// ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {

                }
            }
            return default(T);
        }

        public async Task<T> GetXmlAsync<T>(string url, CancellationToken cancellationToken)
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            httpBaseProtocolFilter.AutomaticDecompression = true;
            var client = new HttpClient(httpBaseProtocolFilter);
            var response = await client.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute)).AsTask(cancellationToken).ConfigureAwait(false);
            if (response != null && (
                response.IsSuccessStatusCode))
            {
                var serializer = new XmlSerializer(typeof(T));

                string res = await response.Content.ReadAsStringAsync().AsTask(cancellationToken).ConfigureAwait(false);
                using (var stringReader = new StringReader(res))
                {
                    var xmlReader = XmlReader.Create(stringReader);
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
            return default(T);
        }

        public async Task<Tuple<HttpStatusCode, string>> CallAsync(string verb,
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            IEnumerable<KeyValuePair<string, string>> content,
            CancellationToken cancellationToken)
        {
            return await CallAsync(verb, url, headers, new HttpFormUrlEncodedContent(content), cancellationToken);
        }

        public async Task<Tuple<HttpStatusCode, string>> CallAsync(string verb,
            string url,
            IEnumerable<KeyValuePair<string, string>> headers,
            IHttpContent content,
            CancellationToken cancellationToken)
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            httpBaseProtocolFilter.AutomaticDecompression = true;
            var client = new HttpClient(httpBaseProtocolFilter);
            var request = new HttpRequestMessage(new HttpMethod(verb), new Uri(url, UriKind.RelativeOrAbsolute));
            if (content != null)
                request.Content = content;
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            var response = await client.SendRequestAsync(request).AsTask(cancellationToken).ConfigureAwait(false);
            if (response != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return new Tuple<HttpStatusCode, string>(response.StatusCode, responseContent);
            }
            return null;
        }
    }
}
