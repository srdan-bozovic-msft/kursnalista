using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using KursnaListaPhoneApp.Resources;
using KursnaLista.Phone.Contracts.Views;
using MSC.Phone.Shared.Contracts.ViewModels;

namespace KursnaListaPhoneApp
{
    public partial class MainPageView : PhoneApplicationPage, IMainPageView
    {
        // Constructor
        public MainPageView()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            // DataContext = App.MainViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (!App.MainViewModel.IsDataLoaded)
            //{
            //    App.MainViewModel.LoadData();
            //}
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        private void RefreshButtonClick(object sender, EventArgs e)
        {
            
        }

        private void ButtonExchangeClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ConverterPage.xaml?from=RSD&to=EUR", UriKind.Relative));
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }
    }
}