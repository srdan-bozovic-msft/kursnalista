using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using KursnaLista.Contracts.Repositories;
using KursnaLista.Contracts.UI.ViewModels;
using KursnaLista.Contracts.UI.Views;
using MSC.Universal.Shared.Contracts.Services;
using MSC.Universal.Shared.UI.Contracts.Services;
using MSC.Universal.Shared.UI.Implementation;

namespace KursnaLista.ViewModels
{
    public class MainPageViewModel : SinglePageViewModel, IMainPageViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IKursnaListaRepository _repository;

        public MainPageViewModel(INavigationService navigationService, IKursnaListaRepository repository, ITimeService timeService, IAnalyticsService analyticsService) 
            : base(navigationService, timeService, analyticsService)
        {
            _navigationService = navigationService;
            _repository = repository;
            ZaDevizeItems = new ObservableCollection<IStavkaKursneListeViewModel>();
            ZaEfektivniStraniNovacItems = new ObservableCollection<IStavkaKursneListeViewModel>();
            SrednjiKursItems = new ObservableCollection<IStavkaKursneListeViewModel>();
            
            GoToConverterCommand = new RelayCommand(() =>
                _navigationService.NavigateTo<IConverterPageView>(new { From = "RSD", To = "EUR" })
                );
            IsDataCurrent = true;
        }

        public ObservableCollection<IStavkaKursneListeViewModel> ZaDevizeItems { get; private set; }
        public ObservableCollection<IStavkaKursneListeViewModel> ZaEfektivniStraniNovacItems { get; private set; }
        public ObservableCollection<IStavkaKursneListeViewModel> SrednjiKursItems { get; private set; }

        private string _datum;

        public string Datum
        {
            get { return _datum; }
            set { Set(ref _datum, value); }
        }

        private bool _isDataCurrent;
        public bool IsDataCurrent
        {
            get
            {
                return _isDataCurrent;
            }
            private set { Set(ref _isDataCurrent, value); }
        }
        public bool IsDataLoaded { get; private set; }

        public ICommand GoToConverterCommand { get; set; }

        public override async void NavigatedTo()
        {
            if (!IsDataLoaded)
            {
                await LoadData();
            }
        }

        protected async Task LoadData()
        {
            var cts = new CancellationTokenSource();

            var result = await _repository.NajnovijaKursnaListaAsync(cts.Token);

            var kursnaListaZaDan = result.Value;
            IsDataCurrent = result.IsCurrent;

            Datum = kursnaListaZaDan.Datum.ToString("d");

            foreach (var item in kursnaListaZaDan.ZaDevize)
            {
                if (!string.IsNullOrEmpty(item.NazivZemlje))
                    ZaDevizeItems.Add(new StavkaKursneListeViewModel(item));
            }

            foreach (var item in kursnaListaZaDan.ZaEfektivniStraniNovac)
            {
                if (!string.IsNullOrEmpty(item.NazivZemlje))
                    ZaEfektivniStraniNovacItems.Add(new StavkaKursneListeViewModel(item));
            }

            foreach (var item in kursnaListaZaDan.SrednjiKurs)
            {
                if (!string.IsNullOrEmpty(item.NazivZemlje))
                    SrednjiKursItems.Add(new StavkaKursneListeViewModel(item));
            }

            IsDataLoaded = true;
        }
    }
}