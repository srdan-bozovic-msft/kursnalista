using System;
using System.Threading;
using System.Threading.Tasks;
using KursnaLista.Contracts.Models;
using KursnaLista.Contracts.Repositories;
using KursnaLista.Contracts.Services.Data;
using MSC.Universal.Shared.Contracts.Repositories;
using MSC.Universal.Shared.Contracts.Services;

namespace KursnaLista.Repositories
{
    public class KursnaListaRepository : IKursnaListaRepository
    {
        private const string KursnaListaLatestDataKey = "kursnaListaLatestData";

        private readonly IKursnaListaDataService _kursnaListaDataService;
        private readonly ICacheService _cacheService;

        public KursnaListaRepository(
            IKursnaListaDataService kursnaListaDataService,
            ICacheService cacheService)
        {
            _kursnaListaDataService = kursnaListaDataService;
            _cacheService = cacheService;
        }

        public async Task<RepositoryResult<KursnaListaZaDan>> NajnovijaKursnaListaAsync(CancellationToken cancellationToken)
        {
            var item = await _cacheService.GetAsync<KursnaListaZaDan>(KursnaListaLatestDataKey).ConfigureAwait(false);
            if (item.HasValue)
            {
                if (cancellationToken.IsCancellationRequested)
                    return RepositoryResult<KursnaListaZaDan>.Create(item.Value, IsCurrent(item.Value.Datum));
                if (IsCurrent(item.Value.Datum))
                {
                    return item.Value;
                }
            }
            var data = await _kursnaListaDataService.GetNajnovijaKursnaListaAsync(cancellationToken).ConfigureAwait(false);
            if (data != null)
            {
                if (IsCurrent(data.Datum))
                {
                    await _cacheService.PutAsync(KursnaListaLatestDataKey, data, DateTime.Now.AddDays(1), DateTime.Now.AddMonths(1)).ConfigureAwait(false);
                }
                else
                {
                    return RepositoryResult<KursnaListaZaDan>.Create(data, false);
                }
            }
            else
            {
                if(item.HasValue)
                {
                    return RepositoryResult<KursnaListaZaDan>.Create(item.Value, false);
                }
                else
                {
                    return RepositoryResult<KursnaListaZaDan>.Create(item.Value, false, false);
                }
            }
            return data;
        }

        public async Task UpdateCacheAsync(CancellationToken cancellationToken)
        {
            var item = await _cacheService.GetAsync<KursnaListaZaDan>(KursnaListaLatestDataKey).ConfigureAwait(false);
            if (item.HasValue && IsCurrent(item.Value.Datum))
            {
                return;
            }
            var data = await _kursnaListaDataService.GetNajnovijaKursnaListaAsync(cancellationToken).ConfigureAwait(false);
            if (data != null)
            {
                await _cacheService.PutAsync(KursnaListaLatestDataKey, data, DateTime.Now.AddDays(1), DateTime.Now.AddMonths(1)).ConfigureAwait(false);
            }
        }

        private bool IsCurrent(DateTime datum)
        {
            var today = DateTime.Now.Date;
            if (datum == today)
                return true;
            if (today.DayOfWeek == DayOfWeek.Saturday
                && datum == today.AddDays(-1))
                return true;
            if (today.DayOfWeek == DayOfWeek.Sunday
                && datum == today.AddDays(-2))
                return true;
            if (DateTime.Now.Hour < 8
                && datum == today.AddDays(-1))
                return true;
            return false;
        }
    }
}
