using KursnaLista.Phone.Contracts.Repositories;
using KursnaLista.Phone.Contracts.Services.Data;
using KursnaLista.Phone.Models;
using MSC.Phone.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KursnaLista.Phone.Repositories
{
    public class KursnaListaRepository : IKursnaListaRepository
    {
        private const string KursnaListaLatestDataKey = "kursnaListaLatestData";

        private IKursnaListaDataService _kursnaListaDataService;
        private ICacheService _cacheService;

        public KursnaListaRepository(
            IKursnaListaDataService kursnaListaDataService,
            ICacheService cacheService)
        {
            _kursnaListaDataService = kursnaListaDataService;
            _cacheService = cacheService;
        }

        public async Task<KursnaListaZaDan> NajnovijaKursnaListaAsync(CancellationToken cancellationToken)
        {
            var item = await _cacheService.GetAsync<KursnaListaZaDan>(KursnaListaLatestDataKey).ConfigureAwait(false);
            if (item.HasValue)
            {
                if (cancellationToken.IsCancellationRequested)
                    return item.Value;
                if (IsCurrent(item))
                {
                    return item.Value;
                }
            }
            var data = await _kursnaListaDataService.GetNajnovijaKursnaListaAsync(cancellationToken).ConfigureAwait(false);
            await _cacheService.PutAsync(KursnaListaLatestDataKey, data).ConfigureAwait(false);
            return data;
        }

        private bool IsCurrent(ICacheItem<KursnaListaZaDan> item)
        {
            var today = DateTime.Now.Date;
            var datum = item.LastSync;
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
