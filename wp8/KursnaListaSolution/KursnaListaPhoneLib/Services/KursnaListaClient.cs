using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursnaListaPhoneLib.Model;
using KursnaListaPhoneLib.Storage;
using MSC.Phone.Common.Networking;
using MSC.Phone.Common.Services;
using Microsoft.Phone.Reactive;

namespace KursnaListaPhoneLib.Services
{
    public class KursnaListaClient : UpdateServiceBase, IKursnaListaClient
    {
        private readonly IHttpClient _client;
        private readonly IKursnaListaStore _store;

        public KursnaListaClient(IHttpClient client, IKursnaListaStore store)
        {
            _client = client;
            _store = store;
        }

        public const string UpdateKursnaListaZaDaneTask = "UpdateKursnaListaZaDane";

        public async Task<TaskCompletedSummary<Unit>> UpdateKursnaListaZaDane(int dana)
        {
            try
            {
                var r = await _client.GetJson<KursnaListaZaDan>("api/latest");
                if (r == null)
                    return new TaskCompletedSummary<Unit>
                    {
                        Task = UpdateKursnaListaZaDaneTask,
                        Result = TaskSummaryResult.NullResponse
                    };

                var result = new List<KursnaListaZaDan>{ r };

                await _store.UpdateKursnaListaZaDane(result);

                return new TaskCompletedSummary<Unit>
                {
                    Task = UpdateKursnaListaZaDaneTask,
                    Result = TaskSummaryResult.Success
                };
            }
            catch (Exception xcp)
            {
                return HandleException<Unit>(UpdateKursnaListaZaDaneTask, xcp);
            }

        }
    }
}
