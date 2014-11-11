using KursnaLista.Contracts.Models;
using KursnaLista.Contracts.UI.ViewModels;

namespace KursnaLista.ViewModels
{
    public class StavkaKursneListeViewModel : IStavkaKursneListeViewModel
    {
        private readonly StavkaKursneListe _model;

        public StavkaKursneListeViewModel(StavkaKursneListe model)
        {
            _model = model;
        }

        public int SifraValute
        {
            get { return _model.SifraValute; }
        }

        public string NazivZemlje
        {
            get { return _model.NazivZemlje; }
        }

        public string OznakaValute
        {
            get { return _model.OznakaValute; }
        }

        public int VaziZa
        {
            get { return _model.VaziZa; }
        }

        public decimal SrednjiKurs
        {
            get { return _model.SrednjiKurs; }
        }

        public decimal KupovniKurs
        {
            get { return _model.KupovniKurs; }
        }

        public decimal ProdajniKurs
        {
            get { return _model.ProdajniKurs; }
        }
    }
}
