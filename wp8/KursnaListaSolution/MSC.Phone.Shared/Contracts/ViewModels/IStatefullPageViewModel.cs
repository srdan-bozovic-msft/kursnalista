using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.ViewModels
{
    public interface IStatefullPageViewModel : IPageViewModel
    {
        Task LoadStateAsync(IDictionary<string, object> state);
        Task SaveStateAsync(IDictionary<string, object> state);
    }
}
