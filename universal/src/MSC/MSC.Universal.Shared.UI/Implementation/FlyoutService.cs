using System.Threading.Tasks;
using MSC.Universal.Shared.Contracts.DI;
using MSC.Universal.Shared.UI.Contracts.Services;
using MSC.Universal.Shared.UI.Contracts.ViewModels;
using MSC.Universal.Shared.UI.Contracts.Views;

namespace MSC.Universal.Shared.UI.Implementation
{
    public class FlyoutService : IFlyoutService
    {
        private readonly IInstanceFactory _instanceFactory;
        private object _currentFlyout;

        public FlyoutService(IInstanceFactory instanceFactory)
        {
            _instanceFactory = instanceFactory;
        }

        public async Task ShowAsync<TF,T>(object parameter = null)
            where TF : IPageView<T>
            where T : IFlyoutViewModel
        {
            var view = _instanceFactory.GetInstance<TF>();
            if (view != null)
            {
                await view.ViewModel.InitializeAsync(parameter);
                var flyout = view as FlyoutViewBase<T>;
                if (flyout != null) flyout.Show();
                _currentFlyout = flyout;
            }
        }

        public async Task ShowIndependentAsync<TF, T>(object parameter = null)
            where TF : IPageView<T>
            where T : IFlyoutViewModel
        {
            var view = _instanceFactory.GetInstance<TF>();
            if (view != null)
            {
                await view.ViewModel.InitializeAsync(parameter);
                var flyout = view as FlyoutViewBase<T>;
                if (flyout != null) flyout.ShowIndependent();
                _currentFlyout = flyout;
            }
        }

        public void HideCurrent<T>()
            where T : IFlyoutViewModel
        {
            var flyout = _currentFlyout as FlyoutViewBase<T>;
            if (flyout != null) flyout.Hide();
        }
    }
}
