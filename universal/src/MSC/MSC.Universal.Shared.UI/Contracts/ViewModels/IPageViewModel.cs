using System.Windows.Input;

namespace MSC.Universal.Shared.UI.Contracts.ViewModels
{
    public interface IPageViewModel : IViewModel
    {
        //Task InitializeAsync(dynamic parameter);
        IErrorViewModel ErrorViewModel { get; }
        ICommand GoBackCommand { get; }
        ICommand GoForwardCommand { get; }
    }
}
