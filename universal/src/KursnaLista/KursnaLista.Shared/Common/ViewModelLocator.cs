using KursnaLista.Contracts.Repositories;
using KursnaLista.Contracts.Services.Data;
using KursnaLista.Contracts.UI.ViewModels;
using KursnaLista.Contracts.UI.Views;
using KursnaLista.Repositories;
using KursnaLista.Services.Data;
using KursnaLista.ViewModels;
using KursnaLista.Views;

using MSC.Universal.Shared.Contracts.DI;
using MSC.Universal.Shared.Contracts.DeviceServices;
using MSC.Universal.Shared.Contracts.Services;
using MSC.Universal.Shared.DI;
using MSC.Universal.Shared.Implementation;
using MSC.Universal.Shared.UI.Contracts.Services;
using MSC.Universal.Shared.UI.Implementation;

namespace KursnaLista.Common
{
    public class ViewModelLocator
    {
        public static IInstanceFactory InstanceFactory
        {
            get
            {
                return SimpleIocInstanceFactory.Default;
            }
        }

        public ViewModelLocator()
        {
            var ioc = InstanceFactory;

            ioc.RegisterType<IHttpClientService, HttpClientService>();
            ioc.RegisterType<ICacheService, LocalStorageCacheService>();
            ioc.RegisterType<IWebBrowser, WebBrowser>();
            ioc.RegisterType<IAnalyticsService, NullAnalyticsService>();
            ioc.RegisterType<ITimeService, TimeService>();
            ioc.RegisterType<ISettingsService, SettingsService>();
            ioc.RegisterType<IDataContext, KursnaListaDataContext>();
            ioc.RegisterType<INavigationService, NavigationService>();
            ioc.RegisterType<ITileService, TileService>();

            ioc.RegisterType<IKursnaListaDataService, KursnaListaDataService>();

            ioc.RegisterType<IKursnaListaRepository, KursnaListaRepository>();
            ioc.RegisterType<IMainPageViewModel, MainPageViewModel>();
            ioc.RegisterType<IConverterPageViewModel, ConverterPageViewModel>();

            ioc.RegisterType<IConverterPageView, ConverterPageView>();
            ioc.RegisterType<IMainPageView, MainPageView>();
        }

        public static INavigationService NavigationService
        {
            get { return InstanceFactory.GetInstance<INavigationService>(); }
        }

        public IMainPageViewModel MainPageViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<IMainPageViewModel>();
            }
        }

        public IConverterPageViewModel ConverterPageViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<IConverterPageViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}