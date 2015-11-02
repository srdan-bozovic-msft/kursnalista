//using System.Windows.Navigation;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Navigation;
//using Microsoft.Phone.Controls;
//using MSC.Universal.Shared.UI.Contracts.ViewModels;
//using MSC.Universal.Shared.UI.Contracts.Views;

//namespace MSC.Phone.Shared.UI.Implementation
//{
//    public abstract class StatefullPhoneApplicationPage : Page, IPageView
//    {
//        protected async override void OnNavigatedTo(NavigationEventArgs e)
//        {
//            base.OnNavigatedTo(e);
//            var statefullPageViewModel = ViewModel as IStatefullPageViewModel;
//            if(statefullPageViewModel != null && State.Count > 0)
//            {
//                if (ViewModel.IsResumingFromTombstoning)
//                {
//                    await statefullPageViewModel.LoadStateAsync(State);
//                }
//                State.Clear();
//            } 
//        }

//        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
//        {
//            base.OnNavigatingFrom(e);
//            if (e.Uri.IsAbsoluteUri)
//            {
//                var statefullPageViewModel = ViewModel as IStatefullPageViewModel;
//                if (statefullPageViewModel != null)
//                {
//                    await statefullPageViewModel.SaveStateAsync(State);
//                }
//            }
//        }

//        public virtual IPageViewModel ViewModel
//        {
//            get
//            {
//                return DataContext as IPageViewModel;
//            }
//        }
//    }
//}
