using System;
using System.Collections.Generic;
using KursnaLista.Mobile.Views;
using KursnaLista.Phone.Contracts.ViewModels;
using KursnaLista.Phone.Contracts.Views;
using Xamarin.Forms;
using MSC.Phone.Shared.Contracts.ViewModels;

namespace KursnaLista.Mobile
{	
	public partial class MainPageView : TabbedPage, IMainPageView
	{	
		public MainPageView ()
		{
			InitializeComponent ();
            BindingContext = App.Locator.MainPageViewModel;

            ToolbarItems.Add(new ToolbarItem("konvertuj", "exchange.png", () =>
            {
                var mainPageViewModel = ViewModel as IMainPageViewModel;
                mainPageViewModel.GoToConverterCommand.Execute(null);
            }));
		}

		protected override void OnAppearing ()
		{
            base.OnAppearing();
            ViewModel.Navigation = Navigation;
			ViewModel.InitializeAsync();
		}

        public IPageViewModel ViewModel
        {
            get { return BindingContext as IPageViewModel; }
        }
    }
}

