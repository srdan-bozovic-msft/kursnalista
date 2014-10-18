using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
//using System.Windows.Navigation;
using System.Threading;
using KursnaLista.Phone.Contracts.Repositories;
using GalaSoft.MvvmLight;
using KursnaLista.Phone.Contracts.ViewModels;
using KursnaLista.Phone.Contracts.Views;
using MSC.Phone.Shared.Contracts.DI;
using MSC.Phone.Shared.Contracts.Services;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using MSC.Phone.Shared.UI.Implementation;
using Xamarin.Forms;

namespace KursnaLista.Phone.ViewModels
{
    public class MainPageViewModel : PageViewModel, IMainPageViewModel
    {
        private readonly IKursnaListaRepository _repository;
        private readonly IInstanceFactory _instanceFactory;

        public MainPageViewModel(
			IKursnaListaRepository repository,
            IInstanceFactory instanceFactory)
        {
            _repository = repository;
            _instanceFactory = instanceFactory;
            ZaDevizeItems = new ObservableCollection<IStavkaKursneListeViewModel>();
            ZaEfektivniStraniNovacItems = new ObservableCollection<IStavkaKursneListeViewModel>();
            SrednjiKursItems = new ObservableCollection<IStavkaKursneListeViewModel>();
			GoToConverterCommand = new RelayCommand(() =>
			{
                var converterView = _instanceFactory.GetInstance<IConverterPageView>();
			    var converterViewModel = converterView.ViewModel as IConverterPageViewModel;
			    converterViewModel.ParameterFrom = "RSD";
			    converterViewModel.ParameterTo = "EUR";
                Navigation.PushAsync(converterView as Page);
			}
			    //_navigationService.Navigate("Converter", new { from = "RSD", to = "EUR" })
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
            set
            {
                Set(ref _datum, value);
            }
        }

        private bool _isDataCurrent;
        public bool IsDataCurrent
        {
            get
            {
                return _isDataCurrent;
            }
            private set
            {
                Set(ref _isDataCurrent, value);
            }
        }
        public bool IsDataLoaded { get; private set; }

        public ICommand GoToConverterCommand { get; set; }

        public async Task InitializeAsync()
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
				try{
                if (!string.IsNullOrEmpty(item.NazivZemlje))
                    ZaDevizeItems.Add(new StavkaKursneListeViewModel(item));
				}
// ReSharper disable once EmptyGeneralCatchClause
				catch
				{

				}
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