using Windows.UI.Xaml.Navigation;
using MSC.Phone.Shared.UI.Implementation;
using MSC.Universal.Shared.UI.Implementation;

namespace MSC.Universal.Shared.UI.Contracts.ViewModels
{
    public interface IMultiPageViewModel
    {
        void NavigatedTo();
        void OnPageDeactivation(NavigatingCancelEventArgs e);
        System.Collections.ObjectModel.ObservableCollection<PageItemViewModel> PageItems { get; }
        int SelectedIndex { get; set; }
    }
}
