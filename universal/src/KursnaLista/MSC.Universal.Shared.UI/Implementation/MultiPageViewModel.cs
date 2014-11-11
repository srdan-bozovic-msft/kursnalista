using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using MSC.Universal.Shared.Contracts.Services;
using MSC.Universal.Shared.UI.Contracts.Services;
using MSC.Universal.Shared.UI.Contracts.ViewModels;

namespace MSC.Universal.Shared.UI.Implementation
{
    public abstract class MultiPageViewModel : SinglePageViewModel, IMultiPageViewModel
    {
        protected MultiPageViewModel(
            INavigationService navigationService,
            ITimeService timeService,
            IAnalyticsService analyticsService,
            bool enableSharing
            )
            : base(navigationService, timeService, analyticsService, enableSharing)
        {
            _pageItems = new ObservableCollection<PageItemViewModel>();
        }

        private readonly ObservableCollection<PageItemViewModel> _pageItems;

        public ObservableCollection<PageItemViewModel> PageItems
        {
            get { return _pageItems; }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                OnSelectedIndexChanging();
                _selectedIndex = value;
                OnSelectedIndexChanged();
                RaisePropertyChanged();
            }
        }

        protected virtual void OnSelectedIndexChanging()
        {

        }

        protected virtual void OnSelectedIndexChanged()
        {

        }

        public async override void NavigatedTo()
        {
            await Task.WhenAll(PageItems.Select(p => p.UpdateAsync()));
        }

        public async override void OnPageDeactivation(NavigatingCancelEventArgs e)
        {
            await Task.WhenAll(PageItems.Select(async p => await p.OnPageDeactivationAsync()));
            base.OnPageDeactivation(e);
        }
    }
}
