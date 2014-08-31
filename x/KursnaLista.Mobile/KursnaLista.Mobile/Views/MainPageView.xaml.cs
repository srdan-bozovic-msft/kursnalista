using System;
using System.Collections.Generic;
using Xamarin.Forms;
using MSC.Phone.Shared.Contracts.ViewModels;

namespace KursnaLista.Mobile
{	
	public partial class MainPageView : ContentPage
	{	
		public MainPageView ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.MainPageViewModel;
		}

		protected async override void OnAppearing ()
		{
			await (BindingContext as IPageViewModel).InitializeAsync (null);
			base.OnAppearing ();
		}
	}
}

