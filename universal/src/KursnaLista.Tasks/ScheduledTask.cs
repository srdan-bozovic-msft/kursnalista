using System.Threading;
using Windows.ApplicationModel.Background;
using KursnaLista.Repositories;
using KursnaLista.Services.Data;
using MSC.Universal.Shared.Implementation;

namespace KursnaLista.Tasks
{
    public sealed class ScheduledTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var httpClientService = new HttpClientService();
            var dataService = new KursnaListaDataService(httpClientService);
            var settingsService = new SettingsService();
            var dataContext = new KursnaListaDataContext(settingsService);
            var cacheService = new LocalStorageCacheService(dataContext);
            var repository = new KursnaListaRepository(dataService, cacheService);

            var cancelationTokenSource = new CancellationTokenSource();

            repository.UpdateCacheAsync(cancelationTokenSource.Token).ContinueWith(_ => deferral.Complete(), cancelationTokenSource.Token);
        }
    }
}
