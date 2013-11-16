using System;
namespace KursnaLista.Phone.Contracts.ViewModels
{
    public interface IStavkaKursneListeViewModel
    {
        decimal KupovniKurs { get; }
        string NazivZemlje { get; }
        string OznakaValute { get; }
        decimal ProdajniKurs { get; }
        int SifraValute { get; }
        decimal SrednjiKurs { get; }
        int VaziZa { get; }
    }
}
