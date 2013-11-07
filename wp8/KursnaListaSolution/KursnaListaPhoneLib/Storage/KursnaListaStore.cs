using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaListaPhoneLib.Model;
using MSC.Phone.Shared.Contracts.Services;
using MSC.Phone.Shared.Implementation;

namespace KursnaListaPhoneLib.Storage
{
    public class KursnaListaStore : IKursnaListaStore
    {
        private ICacheService _cacheService;
        private const string KursnaListaZaDaneKey = "KursnaListaZaDane";

        public KursnaListaStore()
        {
            _cacheService = new PhoneStorageCacheService();
        }

        public async Task<List<KursnaListaZaDan>> GetKursnaListaZaDane()
        {
            return (await _cacheService.GetAsync<List<KursnaListaZaDan>>(KursnaListaZaDaneKey)).Value;
        }

        public async Task<bool> KursnaListaZaDaneNeedsUpdate()
        {
            var item = await _cacheService.GetAsync<List<KursnaListaZaDan>>(KursnaListaZaDaneKey);

            if (item.LastSync == DateTime.MinValue)
                return true;
            var today = DateTime.Now.Date;
            var datum = item.Value.Last().Datum;
            if (datum == today)
                return false;
            if (today.DayOfWeek == DayOfWeek.Saturday
                && datum == today.AddDays(-1))
                return false;
            if (today.DayOfWeek == DayOfWeek.Sunday
                && datum == today.AddDays(-2))
                return false;
            if (DateTime.Now.Hour < 8
                && datum == today.AddDays(-1))
                return false;
            return true;
        }

        public async Task UpdateKursnaListaZaDane(List<KursnaListaZaDan> kursnaListaZaDan)
        {
            await _cacheService.PutAsync(KursnaListaZaDaneKey, kursnaListaZaDan);
        }
    }
}
