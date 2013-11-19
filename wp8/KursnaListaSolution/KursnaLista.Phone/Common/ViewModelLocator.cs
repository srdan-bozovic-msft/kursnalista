/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:KursnaLista.Phone"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using KursnaLista.Phone.Contracts.Repositories;
using KursnaLista.Phone.Contracts.Services.Data;
using KursnaLista.Phone.Contracts.ViewModels;
using KursnaLista.Phone.Repositories;
using KursnaLista.Phone.Services.Data;
using KursnaLista.Phone.ViewModels;
using Microsoft.Practices.ServiceLocation;
using MSC.Phone.Shared.Contracts.DI;
using MSC.Phone.Shared.Contracts.PhoneServices;
using MSC.Phone.Shared.Contracts.Services;
using MSC.Phone.Shared.DI;
using MSC.Phone.Shared.Implementation;

namespace KursnaLista.Phone.Common
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public static IInstanceFactory InstanceFactory
        {
            get
            {
                return SimpleIocInstanceFactory.Default;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            var ioc = InstanceFactory;

            ioc.RegisterType<IHttpClientService, HttpClientService>();
            ioc.RegisterType<ICacheService, PhoneStorageCacheService>();
            ioc.RegisterType<INavigationService, NavigationService>();
            ioc.RegisterType<ITileService, TileService>();

            ioc.RegisterType<IKursnaListaDataService, KursnaListaDataService>();

            ioc.RegisterType<IKursnaListaRepository, KursnaListaRepository>();
            ioc.RegisterType<IMainPageViewModel, MainPageViewModel>();
            ioc.RegisterType<IConverterPageViewModel, ConverterPageViewModel>();
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