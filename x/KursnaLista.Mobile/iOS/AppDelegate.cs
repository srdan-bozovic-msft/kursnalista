using System;
using System.Collections.Generic;
using System.Linq;
using KursnaLista.Mobile.Common;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MSC.Phone.Shared.Contracts.PhoneServices;
using Xamarin.Forms;
using MSC.Phone.Shared.Contracts.Services;
using MSC.Phone.Shared;

namespace KursnaLista.Mobile.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Forms.Init ();

			ViewModelLocator.InstanceFactory.RegisterType<ICacheService, NullCacheService>();
            ViewModelLocator.InstanceFactory.RegisterType<ITileService, NullTileService>();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			window.RootViewController = App.GetMainPage ().CreateViewController ();
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

