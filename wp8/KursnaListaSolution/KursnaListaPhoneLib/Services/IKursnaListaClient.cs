using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaListaPhoneLib.Storage;
using MSC.Phone.Common.Networking;
using MSC.Phone.Common.Services;
using Microsoft.Phone.Reactive;

namespace KursnaListaPhoneLib.Services
{
    public interface IKursnaListaClient
    {
        Task<TaskCompletedSummary<Unit>> UpdateKursnaListaZaDane(int dana); 
    }
}
