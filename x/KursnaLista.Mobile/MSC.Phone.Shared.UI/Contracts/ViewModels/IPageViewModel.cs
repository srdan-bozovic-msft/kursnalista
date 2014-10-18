using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSC.Phone.Shared.Contracts.ViewModels
{
    public interface IPageViewModel
    {
        Task InitializeAsync();

        INavigation Navigation { get; set; }
    }
}
