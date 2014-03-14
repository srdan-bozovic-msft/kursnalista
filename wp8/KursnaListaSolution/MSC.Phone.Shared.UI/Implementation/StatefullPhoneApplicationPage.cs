using Microsoft.Phone.Controls;
using MSC.Phone.Shared.Contracts.ViewModels;
using MSC.Phone.Shared.Contracts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MSC.Phone.Shared
{
    public abstract class StatefullPhoneApplicationPage : PhoneApplicationPage, IPageView
    {
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var statefullPageViewModel = ViewModel as IStatefullPageViewModel;
            if(statefullPageViewModel != null && State.Count > 0)
            {
                await statefullPageViewModel.LoadStateAsync(State);
                State.Clear();
            } 
        }

        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.Uri.IsAbsoluteUri)
            {
                var statefullPageViewModel = ViewModel as IStatefullPageViewModel;
                if (statefullPageViewModel != null)
                {
                    await statefullPageViewModel.SaveStateAsync(State);
                }
            }
        }

        public virtual IPageViewModel ViewModel
        {
            get
            {
                return DataContext as IPageViewModel;
            }
        }
    }
}
