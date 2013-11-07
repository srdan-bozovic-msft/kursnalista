using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaListaPhoneLib.Storage;
using Microsoft.Phone.Reactive;
using System.Threading;

namespace KursnaListaPhoneLib.Services
{
    public interface IKursnaListaClient
    {
        Task UpdateKursnaListaZaDane(int dana, CancellationToken cancellationToken); 
    }
}
