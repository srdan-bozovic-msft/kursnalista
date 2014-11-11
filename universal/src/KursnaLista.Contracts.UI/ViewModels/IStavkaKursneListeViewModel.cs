namespace KursnaLista.Contracts.UI.ViewModels
{
    public interface IStavkaKursneListeViewModel
    {
        string NazivZemlje { get; }
        string OznakaValute { get; }
        int SifraValute { get; }
        int VaziZa { get; }
        decimal KupovniKurs { get; }
        decimal ProdajniKurs { get; }
        decimal SrednjiKurs { get; }
    }
}
