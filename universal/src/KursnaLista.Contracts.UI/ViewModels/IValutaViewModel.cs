namespace KursnaLista.Contracts.UI.ViewModels
{
    public interface IValutaViewModel
    {
        string Naziv { get; }

        string Oznaka { get; }

        decimal SrednjiKurs { get; }

        int VaziZa { get; }
    }
}
