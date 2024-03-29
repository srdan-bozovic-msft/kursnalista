﻿using KursnaLista.Mobile.Views;
using KursnaLista.Phone.Contracts.Repositories;
using KursnaLista.Phone.Contracts.Services.Data;
using KursnaLista.Phone.Contracts.ViewModels;
using KursnaLista.Phone.Contracts.Views;
using KursnaLista.Phone.Repositories;
using KursnaLista.Phone.Services.Data;
using KursnaLista.Phone.ViewModels;
using MSC.Phone.Shared;
using MSC.Phone.Shared.Contracts.DI;
using MSC.Phone.Shared.Contracts.Services;
using MSC.Phone.Shared.DI;

namespace KursnaLista.Mobile.Common
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

		/// <summary>
		/// Initializes a new instance of the ViewModelLocator class.
		/// </summary>
		public ViewModelLocator()
		{
			var ioc = InstanceFactory;

			ioc.RegisterType<IHttpClientService, HttpClientService>();
			//ioc.RegisterType<ICacheService, NullCacheService>();
			//ioc.RegisterType<ICacheService, PhoneStorageCacheService>();
			//ioc.RegisterType<INavigation, NavigationService>();
			//ioc.RegisterType<ITileService, TileService>();

			ioc.RegisterType<IKursnaListaDataService, KursnaListaDataService>();

			ioc.RegisterType<IKursnaListaRepository, KursnaListaRepository>();

			ioc.RegisterType<IMainPageViewModel, MainPageViewModel>();
			ioc.RegisterType<IConverterPageViewModel, ConverterPageViewModel>();

            ioc.RegisterType<IMainPageView, MainPageView>();
            ioc.RegisterType<IConverterPageView, ConverterPageView>();

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

