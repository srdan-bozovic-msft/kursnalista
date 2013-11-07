using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharpGIS;

namespace MSC.Phone.Common.Networking
{
    public class HttpClient : IHttpClient
    {
        private readonly string _baseUrl;

        public HttpClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<T> GetJson<T>(string url)
        {
            var client = new GZipWebClient();
            var response = await client.DownloadStringTask(_baseUrl + url);
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
