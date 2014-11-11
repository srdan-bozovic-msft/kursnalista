using KursnaLista.Contracts.UI.Views;
using KursnaLista.ViewModels;
using MSC.Universal.Shared.UI.Contracts.ViewModels;
using MSC.Universal.Shared.UI.Implementation;

namespace KursnaLista.Views
{
    public partial class ConverterPageView : ConverterPageViewBase, IConverterPageView
    {
        public ConverterPageView()
        {
            InitializeComponent();
        }
        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }
    }

    public class ConverterPageViewBase : PageViewBase<ConverterPageViewModel>
    {

    }
}