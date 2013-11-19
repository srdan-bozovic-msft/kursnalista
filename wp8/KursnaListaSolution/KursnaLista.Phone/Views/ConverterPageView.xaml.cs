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
using KursnaLista.Phone.Contracts.Views;
using MSC.Phone.Shared.Contracts.ViewModels;

namespace KursnaLista.Phone.Views
{
    public partial class ConverterPageView : PhoneApplicationPage, IConverterPageView
    {
        public ConverterPageView()
        {
            InitializeComponent();
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }
    }
}