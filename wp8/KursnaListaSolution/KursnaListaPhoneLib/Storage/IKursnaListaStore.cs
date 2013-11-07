using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaListaPhoneLib.Model;

namespace KursnaListaPhoneLib.Storage
{
    public interface IKursnaListaStore
    {
        Task<List<KursnaListaZaDan>> GetKursnaListaZaDane();
        Task<bool> KursnaListaZaDaneNeedsUpdate();
        Task UpdateKursnaListaZaDane(List<KursnaListaZaDan> kursnaListaZaDane);
    }
}
