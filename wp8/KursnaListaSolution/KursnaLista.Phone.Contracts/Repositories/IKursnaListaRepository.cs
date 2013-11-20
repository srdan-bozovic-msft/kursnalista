using KursnaLista.Phone.Models;
using MSC.Phone.Shared.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KursnaLista.Phone.Contracts.Repositories
{
    public interface IKursnaListaRepository
    {
        Task<RepositoryResult<KursnaListaZaDan>> NajnovijaKursnaListaAsync(CancellationToken cancellationToken);
        Task UpdateCache(CancellationToken cancellationToken);
    }
}
