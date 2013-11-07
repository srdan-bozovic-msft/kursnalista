using MSC.Phone.Shared.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.Views
{
    public interface IPageView
    {
        IPageViewModel ViewModel { get; }
    }
}
