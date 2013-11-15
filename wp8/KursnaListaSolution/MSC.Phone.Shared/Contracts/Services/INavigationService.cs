using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.Services
{
    public interface INavigationService
    {
        void Navigate(string pageKey);
        void Navigate(string pageKey, object parameter);
        void GoBack();
    }
}
