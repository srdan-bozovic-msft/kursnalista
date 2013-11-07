using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaListaPhoneLib.Model;
using KursnaListaPhoneLib.Storage;
using Microsoft.Phone.Reactive;
using MSC.Phone.Shared.Contracts.Services;
using System.Threading;

namespace KursnaListaPhoneLib.Services
{
    public class KursnaListaClient : IKursnaListaClient
    {
        private readonly IHttpClientService _client;
        private readonly IKursnaListaStore _store;
        private const string BaseUrl = "https://kursna-lista.azure-mobile.net/";

        public KursnaListaClient(IHttpClientService client, IKursnaListaStore store)
        {
            _client = client;
            _store = store;
        }

        public const string UpdateKursnaListaZaDaneTask = "UpdateKursnaListaZaDane";

        public async Task UpdateKursnaListaZaDane(int dana, CancellationToken cancellationToken)
        {
            var latest = await _client.GetJsonAsync<KursnaListaZaDan>(BaseUrl+"api/latest", cancellationToken);        
   
            if(latest == null)
                throw new Exception();

            var result = new List<KursnaListaZaDan> {
                latest
            };

            await _store.UpdateKursnaListaZaDane(result);
        }
    }
}
