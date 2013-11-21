using KursnaLista.Phone.Contracts.Repositories;
using KursnaLista.Phone.Models;
using MSC.Phone.Shared.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestApp.Mocks.Repositories
{
    public class MockKursnaListaRepository : IKursnaListaRepository
    {
        private KursnaListaZaDan _kursnaListZaDan;
        public MockKursnaListaRepository(KursnaListaZaDan kursnaListaZaDan)
        {
            _kursnaListZaDan = kursnaListaZaDan;
        }

        public async Task<RepositoryResult<KursnaListaZaDan>> NajnovijaKursnaListaAsync(CancellationToken cancellationToken)
        {
            return RepositoryResult<KursnaListaZaDan>.Create(_kursnaListZaDan, true);
        }

        public async Task UpdateCache(System.Threading.CancellationToken cancellationToken)
        {
             
        }
    }
}
