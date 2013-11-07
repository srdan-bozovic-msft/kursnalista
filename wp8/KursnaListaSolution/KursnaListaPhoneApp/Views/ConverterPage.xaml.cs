using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace KursnaListaPhoneApp.Views
{
    public partial class ConverterPage : PhoneApplicationPage
    {
        private const string PinAppBarUrl = "/Assets/AppBar/pin.png";
        private const string UnpinAppBarUrl = "/Assets/AppBar/unpin.png";

        public ConverterPage()
        {
            InitializeComponent();
            // Set the data context of the listbox control to the sample data
            DataContext = App.ConverterViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ConverterViewModel.IsDataLoaded)
            {
                var from = NavigationContext.QueryString["from"];
                var to = NavigationContext.QueryString["to"];

                App.ConverterViewModel.PinModeChanged += (_, __) => SetPinButton(App.ConverterViewModel.PinMode);
                App.ConverterViewModel.LoadData(from, to);
            }
        }

        private void ButtonPinToStartClick(object sender, EventArgs e)
        {
            App.ConverterViewModel.SetTileCommand.Execute(null);
        }

        public ApplicationBarIconButton pinBtn
        {
            get
            {
                var appBar = (ApplicationBar)ApplicationBar;
                var count = appBar.Buttons.Count;
                for (var i = 0; i < count; i++)
                {
                    ApplicationBarIconButton btn = appBar.Buttons[i] as ApplicationBarIconButton;
                    if (btn.IconUri.OriginalString.Contains("pin"))
                        return btn;
                }
                return null;
            }
        }

        void SetPinButton(bool pin)
        {
            if(pin)
            {
                pinBtn.IconUri = new Uri(PinAppBarUrl, UriKind.Relative);
                pinBtn.Text = "zakači";
            }
            else
            {
                pinBtn.IconUri = new Uri(UnpinAppBarUrl, UriKind.Relative);
                pinBtn.Text = "otkači";
            }
        }
    }
}