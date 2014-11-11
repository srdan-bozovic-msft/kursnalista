using System.Threading;
using System.Threading.Tasks;
using KursnaLista.Contracts.Models;

namespace KursnaLista.Contracts.Services.Data
{
    public interface IKursnaListaDataService
    {
        Task<KursnaListaZaDan> GetNajnovijaKursnaListaAsync(CancellationToken cancellationToken);
    }
}
