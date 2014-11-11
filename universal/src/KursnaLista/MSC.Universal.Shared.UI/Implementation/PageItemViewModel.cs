using System.Threading.Tasks;

namespace MSC.Universal.Shared.UI.Implementation
{
    public abstract class PageItemViewModel : ViewModel
    {
        public MultiPageViewModel ParentViewModel { get; private set; }
        
        private string _title;
        public string Title
        {
            get { return _title.ToLower(); }
            set { Set(ref _title, value); }
        }

        protected PageItemViewModel(MultiPageViewModel parentViewModel, string title)
        {
            ParentViewModel = parentViewModel;
            Title = title;
        }

        public T GetParent<T>()
            where T : MultiPageViewModel
        {
            return ParentViewModel as T;
        }

        public abstract Task UpdateAsync(bool forceRefresh = false);

        protected override void OnLoading()
        {
            ParentViewModel.IsLoading = IsLoading;
            base.OnLoading();
        }

        // ReSharper disable once CSharpWarnings::CS1998
        public async virtual Task OnPageDeactivationAsync()
        {
            
        }
    }
}
