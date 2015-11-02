namespace MSC.Universal.Shared.UI.Contracts.ViewModels
{
    public interface IErrorViewModel
    {
        bool IsError { get; }
        string ErrorMessage { get; }
        string ErrorImage { get; }
        void SetError(string message, string image);
        void ClearError();
    }
}
