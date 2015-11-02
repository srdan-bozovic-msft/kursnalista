using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSC.Universal.Shared.UI.Contracts.ViewModels
{
    public interface IStatefullPageViewModel : IPageViewModel
    {
        Task LoadStateAsync(IDictionary<string, object> state);
        Task SaveStateAsync(IDictionary<string, object> state);
    }
}
