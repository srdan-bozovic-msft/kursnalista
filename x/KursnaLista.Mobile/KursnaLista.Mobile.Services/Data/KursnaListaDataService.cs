using KursnaLista.Phone.Contracts.Services.Data;
using KursnaLista.Phone.Models;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KursnaLista.Phone.Services.Data
{
    public class KursnaListaDataService : IKursnaListaDataService
    {
        private const string BaseUrl = "https://kursna-lista.azure-mobile.net";

        private IHttpClientService _httpClientService;

        public KursnaListaDataService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<KursnaListaZaDan> GetNajnovijaKursnaListaAsync(CancellationToken cancellationToken)
        {
            return await _httpClientService.GetJsonAsync<KursnaListaZaDan>(BaseUrl + "/api/latest", cancellationToken).ConfigureAwait(false);
        }
    }
}
