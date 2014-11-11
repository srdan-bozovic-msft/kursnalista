using Windows.UI.Xaml.Controls;
using KursnaLista.Contracts.UI.ViewModels;
using KursnaLista.Contracts.UI.Views;
using KursnaLista.ViewModels;
using MSC.Universal.Shared.UI.Contracts.ViewModels;
using MSC.Universal.Shared.UI.Implementation;

namespace KursnaLista.Views
{
    public partial class MainPageView : MainPageViewBase, IMainPageView
    {
        // Constructor
        public MainPageView()
        {
            InitializeComponent();
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }
    }

    public class MainPageViewBase : PageViewBase<MainPageViewModel>
    {
        
    }
}