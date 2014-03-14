using System;
namespace KursnaLista.Phone.Contracts.ViewModels
{
    public interface IValutaViewModel
    {
        string Naziv { get; }

        string Oznaka { get; }

        decimal SrednjiKurs { get; }

        int VaziZa { get; }
    }
}
