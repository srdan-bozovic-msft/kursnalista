using KursnaLista.Contracts.Models;
using KursnaLista.Contracts.UI.ViewModels;

namespace KursnaLista.ViewModels
{
    public class ValutaViewModel : IValutaViewModel
    {
        private readonly StavkaKursneListe _model;

        public ValutaViewModel(StavkaKursneListe model)
        {
            _model = model;
        }
        public string Naziv
        {
            get { return string.Format("{0} ({1})", _model.NazivZemlje, _model.OznakaValute); }
        }

        public string Oznaka
        {
            get { return _model.OznakaValute; }
        }

        public decimal SrednjiKurs
        {
            get { return _model.SrednjiKurs; }
        }

        public int VaziZa
        {
            get { return _model.VaziZa; }
        }
    }
}
