using System.Threading.Tasks;
using System.Windows.Input;

namespace MSC.Universal.Shared.UI.Contracts.ViewModels
{
    public interface IPageViewModel : IViewModel
    {
        //Task InitializeAsync(dynamic parameter);

        ICommand GoBackCommand { get; }
        ICommand GoForwardCommand { get; }
    }
}
