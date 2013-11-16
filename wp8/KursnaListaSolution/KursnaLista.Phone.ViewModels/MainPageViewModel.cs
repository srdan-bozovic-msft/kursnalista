using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Threading;
using KursnaLista.Phone.Contracts.Repositories;
using GalaSoft.MvvmLight;
using KursnaLista.Phone.Contracts.ViewModels;

namespace KursnaLista.Phone.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        private readonly IKursnaListaRepository _repository;

        public MainPageViewModel(IKursnaListaRepository repository)
        {
            _repository = repository;
            this.ZaDevizeItems = new ObservableCollection<IStavkaKursneListeViewModel>();
            this.ZaEfektivniStraniNovacItems = new ObservableCollection<IStavkaKursneListeViewModel>();
            this.SrednjiKursItems = new ObservableCollection<IStavkaKursneListeViewModel>();
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
                this._datum = value;
                RaisePropertyChanged("Datum"); 
            }
        }

        public bool IsDataLoaded { get; private set; }

        public async Task InitializeAsync(dynamic parameter)
        {
            if (!IsDataLoaded)
            {
                await LoadData();
            }
        }

        protected async Task LoadData()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            var kursnaListaZaDan = await _repository.NajnovijaKursnaListaAsync(cts.Token);

            Datum = kursnaListaZaDan.Datum.ToShortDateString();

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

            this.IsDataLoaded = true;
            return;
        }
    }
}