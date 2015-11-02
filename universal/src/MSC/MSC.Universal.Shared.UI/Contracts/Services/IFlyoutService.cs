using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSC.Universal.Shared.UI.Contracts.ViewModels;
using MSC.Universal.Shared.UI.Contracts.Views;
using MSC.Universal.Shared.UI.Implementation;

namespace MSC.Universal.Shared.UI.Contracts.Services
{
    public interface IFlyoutService
    {
        Task ShowAsync<TF, T>(object parameter = null)
            where TF : IPageView<T>
            where T : IFlyoutViewModel;

        Task ShowIndependentAsync<TF, T>(object parameter = null)
            where TF : IPageView<T>
            where T : IFlyoutViewModel;

        void HideCurrent<T>()
            where T : IFlyoutViewModel;
    }
}
