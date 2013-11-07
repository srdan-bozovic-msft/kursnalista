using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaListaPhoneLib.Model;
using MSC.Phone.Common.Storage;

namespace KursnaListaPhoneLib.Storage
{
    public class KursnaListaStore : StoreBase, IKursnaListaStore
    {
        private const string KursnaListaZaDaneKey = "KursnaListaZaDane";

        public KursnaListaStore()
        {
            
        }

        protected override async Task Load()
        {
            await LoadComplexObject<List<KursnaListaZaDan>>(KursnaListaZaDaneKey);
        }

        public async Task<List<KursnaListaZaDan>> GetKursnaListaZaDane()
        {
            return await GetValue<List<KursnaListaZaDan>>(KursnaListaZaDaneKey);
        }

        public async Task<bool> KursnaListaZaDaneNeedsUpdate()
        {

                if (await GetLastSync(KursnaListaZaDaneKey) == DateTime.MinValue)
                    return true;
                var today = DateTime.Now.Date;
                var datum = (await GetKursnaListaZaDane()).Last().Datum;
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
            await StoreComplexObject(KursnaListaZaDaneKey, kursnaListaZaDan);
        }
    }
}
