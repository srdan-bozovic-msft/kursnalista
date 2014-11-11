using System.Threading;
using System.Threading.Tasks;
using KursnaLista.Contracts.Models;
using KursnaLista.Contracts.Services.Data;
using MSC.Universal.Shared.Contracts.Services;

namespace KursnaLista.Services.Data
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
