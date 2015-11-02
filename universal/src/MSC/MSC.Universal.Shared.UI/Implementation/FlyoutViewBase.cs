using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using MSC.Universal.Shared.UI.Contracts.ViewModels;
using MSC.Universal.Shared.UI.Contracts.Views;

namespace MSC.Universal.Shared.UI.Implementation
{
    public class FlyoutViewBase<T> : SettingsFlyout, IPageView<T>
        where T : IFlyoutViewModel
    {
        public FlyoutViewBase()
        {
            Loaded += (sender, e) =>
            {
                ViewModel.NavigatedTo();
            };

            // Undo the same changes when the page is no longer visible
            Unloaded += (sender, e) =>
            {
                ViewModel.OnPageDeactivation(null);
            };            
        }

        private T _viewModel;

        public T ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = (T) DataContext;
                }
                return _viewModel;
            }
        }

        private bool _back;
        private Popup _popup;

        public static readonly DependencyProperty IsLightDismissedEnabledProperty =
            DependencyProperty.Register(
                "IsLightDismissedEnabled",
                typeof (bool),
                typeof (FlyoutViewBase<T>),
                new PropertyMetadata(true, IsLightDismissedEnabledPropertyChangedCallback));

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(
                "IsOpen",
                typeof (bool),
                typeof (FlyoutViewBase<T>),
                new PropertyMetadata(true, IsOpenPropertyChangedCallback));

        private static void IsOpenPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                var flyoutViewBase = d as FlyoutViewBase<T>;
                if (flyoutViewBase != null)
                {
                    flyoutViewBase._popup.IsOpen = (bool) e.NewValue;
                    flyoutViewBase.IsOpen = (bool) e.NewValue;
                }
            }
        }

        private static void IsLightDismissedEnabledPropertyChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                var flyoutViewBase = d as FlyoutViewBase<T>;
                if (flyoutViewBase != null)
                {
                    flyoutViewBase._popup.IsLightDismissEnabled = (bool) e.NewValue;
                    flyoutViewBase.IsLightDismissedEnabled = (bool) e.NewValue;
                    if ((bool) e.NewValue)
                    {
                        flyoutViewBase._popup.IsOpen = false;
                        flyoutViewBase._popup.IsOpen = true;
                    }
                }
            }
        }

        public bool IsLightDismissedEnabled
        {
            get { return (bool) GetValue(IsLightDismissedEnabledProperty); }
            set { SetValue(IsLightDismissedEnabledProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool) GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public new void ShowIndependent()
        {
            base.ShowIndependent();
            _popup = (Parent as Popup);
            if (_popup != null) _popup.Closed += Popup_Closed;
            BackClick += FlyoutViewBase_BackClick;
        }

        public new void Show()
        {
            base.Show();
            _popup = (Parent as Popup);
            if (_popup != null) _popup.Closed += Popup_Closed;
            BackClick += FlyoutViewBase_BackClick;
        }

        private void FlyoutViewBase_BackClick(object sender, BackClickEventArgs e)
        {
            _back = true;
        }

        private void Popup_Closed(object sender, object e)
        {
            if (!_back && IsLightDismissedEnabled == false)
            {
                _popup.IsOpen = true;
            }
            else
            {
                _popup = (Parent as Popup);
                if (_popup != null) _popup.Closed -= Popup_Closed;
            }
        }
    }
}
