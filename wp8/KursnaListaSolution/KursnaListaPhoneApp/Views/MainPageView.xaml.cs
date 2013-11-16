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
using KursnaLista.Phone.Contracts.ViewModels;

namespace KursnaListaPhoneApp
{
    public partial class MainPageView : PhoneApplicationPage, IMainPageView
    {
        // Constructor
        public MainPageView()
        {
            InitializeComponent();
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }
    }
}