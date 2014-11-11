using System.Threading;
using System.Threading.Tasks;
using KursnaLista.Contracts.Models;
using MSC.Universal.Shared.Contracts.Repositories;

namespace KursnaLista.Contracts.Repositories
{
    public interface IKursnaListaRepository
    {
        Task<RepositoryResult<KursnaListaZaDan>> NajnovijaKursnaListaAsync(CancellationToken cancellationToken);
        Task UpdateCacheAsync(CancellationToken cancellationToken);
    }
}
