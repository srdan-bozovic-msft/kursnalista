using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.StartScreen;
using GalaSoft.MvvmLight.Command;
using KursnaLista.Contracts.Models;
using KursnaLista.Contracts.Repositories;
using KursnaLista.Contracts.UI.ViewModels;

using MSC.Universal.Shared.Contracts.PhoneServices;
using MSC.Universal.Shared.Contracts.Services;
using MSC.Universal.Shared.UI.Contracts.Services;
using MSC.Universal.Shared.UI.Implementation;

namespace KursnaLista.ViewModels
{
    public class ConverterPageViewModel : SinglePageViewModel, IConverterPageViewModel
    {
        private readonly IKursnaListaRepository _repository;
        private readonly ITileService _tileService;

        public ConverterPageViewModel(IKursnaListaRepository repository, ITileService tileService,
            INavigationService navigationService, ITimeService timeService, IAnalyticsService analyticsService)
            : base(navigationService, timeService, analyticsService)
        {
            _repository = repository;
            _tileService = tileService;
            ValutaIzItems = new ObservableCollection<IValutaViewModel>();
            ValutaUItems = new ObservableCollection<IValutaViewModel>();
            KonvertujCommand = new RelayCommand(
                () =>
                {
                    Result = (string.IsNullOrEmpty(Iznos) ? 0 : Convert.ToDecimal(Iznos))*
                             (ValutaIzItems[ValutaIzIndex].SrednjiKurs/ValutaIzItems[ValutaIzIndex].VaziZa)/
                             (ValutaUItems[ValutaUIndex].SrednjiKurs/ValutaUItems[ValutaUIndex].VaziZa);
                },
                () => ValutaIzIndex != -1 && ValutaUIndex != -1);
            SetTileCommand = new RelayCommand(async ()=> await SetTileAsync());
            IsDataCurrent = true;
        }

        public ObservableCollection<IValutaViewModel> ValutaIzItems { get; private set; }
        public ObservableCollection<IValutaViewModel> ValutaUItems { get; private set; }

        private int _valutaIzIndex = -1;

        public int ValutaIzIndex
        {
            get { return _valutaIzIndex; }
            set
            {
                Set(ref _valutaIzIndex, value);
                OnPinModeChanged();
            }
        }


        private int _valutaUIndex = -1;

        public int ValutaUIndex
        {
            get { return _valutaUIndex; }
            set
            {
                Set(ref _valutaUIndex, value);
                OnPinModeChanged();
            }
        }

        private string _iznos;

        public string Iznos
        {
            get { return _iznos; }
            set
            {
                Set(ref _iznos, value);
            }
        }

        private decimal _result;

        public decimal Result
        {
            get { return _result; }
            set
            {
                Set(ref _result, value);
            }
        }

        public ICommand KonvertujCommand { get; set; }
        public ICommand SetTileCommand { get; set; }

        private bool _isDataCurrent;

        public bool IsDataCurrent
        {
            get { return _isDataCurrent; }
            private set
            {
                Set(ref _isDataCurrent, value);
            }
        }

        public bool IsDataLoaded { get; private set; }

        public async override void OnNavigatedForwardToView(dynamic parameter)
        {
            await LoadData(NavigationService.GetParameter<string>("From"), NavigationService.GetParameter<string>("To"));
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task LoadData(string from, string to)
        {
            var cts = new CancellationTokenSource();

            var result = await _repository.NajnovijaKursnaListaAsync(cts.Token);

            var kursnaListaZaDan = result.Value;
            IsDataCurrent = result.IsCurrent;

            var items = kursnaListaZaDan.SrednjiKurs.OrderBy(s => s.NazivZemlje).ToList();
            items.Insert(0,
                new StavkaKursneListe
                {
                    NazivZemlje = "Srbija",
                    OznakaValute = "RSD",
                    SrednjiKurs = 1.0M,
                    VaziZa = 1
                });

            var fromIndex = -1;
            var toIndex = -1;

            var index = 0;
            foreach (var item in items)
            {
                if (item.OznakaValute == from)
                    fromIndex = index;
                if (item.OznakaValute == to)
                    toIndex = index;
                ValutaIzItems.Add(new ValutaViewModel(item));
                ValutaUItems.Add(new ValutaViewModel(item));
                index++;
            }

            _valutaIzIndex = fromIndex;
            _valutaUIndex = toIndex;

            RaisePropertyChanged(() => ValutaUIndex);
            RaisePropertyChanged(() => ValutaIzIndex);
            OnPinModeChanged();
            var relayCommand = KonvertujCommand as RelayCommand;
            if (relayCommand != null) relayCommand.RaiseCanExecuteChanged();

            IsDataLoaded = true;
        }

        private void OnPinModeChanged()
        {
            //RaisePropertyChanged(() => SetTileButtonIconUri);
            //RaisePropertyChanged(() => SetTileButtonText);
            RaisePropertyChanged(() => PinMode);
        }

        //public Uri SetTileButtonIconUri
        //{
        //    get
        //    {
        //        return PinMode ? new Uri("ms-appx:///Assets/AppBar/unpin.png") : new Uri("ms-appx:///Assets/AppBar/pin.png");
        //    }
        //}

        //public string SetTileButtonText
        //{
        //    get
        //    {
        //        return PinMode ? "zakači" : "otkači";
        //    }
        //}

        public bool PinMode
        {
            get
            {
                if (ValutaIzIndex == -1 || ValutaUIndex == -1)
                    return true;
                var from = ValutaIzItems[ValutaIzIndex].Oznaka;
                var to = ValutaUItems[ValutaUIndex].Oznaka;
                return !TileExists(from, to);
            }
        }

        private async Task SetTileAsync()
        {
            if (ValutaIzIndex == -1 || ValutaUIndex == -1)
                return;
            var from = ValutaIzItems[ValutaIzIndex].Oznaka;
            var to = ValutaUItems[ValutaUIndex].Oznaka;
            if (TileExists(from, to))
            {
                await DeleteTileAsync(from, to);
            }
            else
            {
                await CreateTileAsync(from, to);
            }
        }

        private bool TileExists(string from, string to)
        {
            var secondaryTileId = string.Format("{0}-{1}", from, to);
            return true;//_tileService.TileExists(secondaryTileId);
        }

        private async Task DeleteTileAsync(string from, string to)
        {
            var secondaryTileId = string.Format("{0}-{1}", from, to);
            //await _tileService.DeleteTileAsync(secondaryTileId);
            OnPinModeChanged();
        }

        private async Task CreateTileAsync(string from, string to)
        {
            //var secondaryTileId = string.Format("{0}-{1}", from, to);
            //var title = string.Format("{0} -> {1}", from, to);
            //var tileActivationArguments = string.Format("{0}:{1}", from, to);
            //var tileLogo = new Uri("ms-appx:///Assets/LogoExchange.png");
            //const TileSize tileSize = TileSize.Square150x150;

            //var secondaryTile = new SecondaryTile(
            //    secondaryTileId,
            //    title,
            //    tileActivationArguments,
            //    tileLogo,
            //    tileSize
            //    );

            //secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            //secondaryTile.RoamingEnabled = true;

            //await _tileService.CreateTileAsync(secondaryTile);
            //OnPinModeChanged();
        }

        //public async override Task LoadStateAsync(IDictionary<string, object> state)
        //{
        //    await LoadData("RSD", "EUR");
        //    ValutaIzIndex = (int)state["ValutaIzIndex"];
        //    ValutaUIndex = (int)state["ValutaUIndex"];
        //    Iznos = (string)state["Iznos"];
        //    Result = (decimal)state["Result"];
        //}

        //public async override Task SaveStateAsync(IDictionary<string, object> state)
        //{
        //    state["ValutaIzIndex"] = ValutaIzIndex;
        //    state["ValutaUIndex"] = ValutaUIndex;
        //    state["Iznos"] = Iznos;
        //    state["Result"] = Result;
        //}
    }
}