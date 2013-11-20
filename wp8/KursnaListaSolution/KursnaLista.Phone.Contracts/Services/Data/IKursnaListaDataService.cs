using KursnaLista.Phone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KursnaLista.Phone.Contracts.Services.Data
{
    public interface IKursnaListaDataService
    {
        Task<KursnaListaZaDan> GetNajnovijaKursnaListaAsync(CancellationToken cancellationToken);
    }
}
