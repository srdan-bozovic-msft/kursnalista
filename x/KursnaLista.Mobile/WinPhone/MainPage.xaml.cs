using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using KursnaLista.Mobile.Common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MSC.Phone.Shared;
using MSC.Phone.Shared.Contracts.PhoneServices;
using MSC.Phone.Shared.Contracts.Services;
using Xamarin.Forms;


namespace KursnaLista.Mobile.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            Forms.Init();

            #region Windows Phone Dependencies

            ViewModelLocator.InstanceFactory.RegisterType<ICacheService, NullCacheService>();
            ViewModelLocator.InstanceFactory.RegisterType<ITileService, TileService>();

            #endregion

            Content = KursnaLista.Mobile.App.GetMainPage().ConvertPageToUIElement(this);
        }
    }
}
