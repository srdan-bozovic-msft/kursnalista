using GalaSoft.MvvmLight;

namespace MSC.Universal.Shared.UI.Implementation
{
    public abstract class PivotPageViewModel : ViewModelBase
    {
        private string _title;

        public string Title
        {
            get { return _title.ToLower(); }
            set { Set(ref _title, value); }
        }
    }
}