using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using KursnaLista.Mobile.Common;
using MSC.Phone.Shared;
using MSC.Phone.Shared.Contracts.PhoneServices;
using Xamarin.Forms.Platform.Android;
using MSC.Phone.Shared.Contracts.Services;
using MSC.Android.Shared;


namespace KursnaLista.Mobile.Android
{
	[Activity (Label = "KursnaLista.Mobile.Android.Android", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : AndroidActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Xamarin.Forms.Forms.Init (this, bundle);

			#region Android Dependencies

			ViewModelLocator.InstanceFactory.RegisterType<ICacheService, FileStorageCacheService>();
            ViewModelLocator.InstanceFactory.RegisterType<ITileService, NullTileService>();

			#endregion

			SetPage (App.GetMainPage ());
		}
	}
}

