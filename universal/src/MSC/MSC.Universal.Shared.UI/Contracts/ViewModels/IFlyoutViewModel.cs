using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml.Navigation;

namespace MSC.Universal.Shared.UI.Contracts.ViewModels
{
    public interface IFlyoutViewModel
    {
        Task InitializeAsync(object parameter);

        void NavigatedTo();

        void OnPageDeactivation(NavigatingCancelEventArgs e);
    }
}
