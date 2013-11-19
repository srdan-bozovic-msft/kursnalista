using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using KursnaLista.Phone.Services.Data;
using MSC.Phone.Shared.Implementation;
using KursnaLista.Phone.Repositories;

namespace KursnaLista.Phone.Agent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected async override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background
            var httpClientService = new HttpClientService();
            var dataService = new KursnaListaDataService(httpClientService);
            var cacheService = new PhoneStorageCacheService();
            var repository = new KursnaListaRepository(dataService, cacheService);

            await repository.UpdateCache();

            NotifyComplete();
        }
    }
}