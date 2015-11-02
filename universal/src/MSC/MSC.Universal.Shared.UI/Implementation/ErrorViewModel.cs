using System.Net.NetworkInformation;
using GalaSoft.MvvmLight;
using MSC.Universal.Shared.UI.Contracts.ViewModels;

namespace MSC.Universal.Shared.UI.Implementation
{
    public class ErrorViewModel : ViewModelBase, IErrorViewModel
    {
        private bool _isError;
        public bool IsError
        {
            get { return _isError; }
            set { Set(ref _isError, value); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(ref _errorMessage, value); }
        }

        private string _errorImage;
        public string ErrorImage
        {
            get { return _errorImage; }
            set { Set(ref _errorImage, value); }
        }

        public void SetError(string message, string image)
        {
            IsError = true;
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                ErrorMessage = "NO INTERNET";
                ErrorImage = "/Assets/images/noconnection.png";
            }
            else
            {
                ErrorMessage = message;
                ErrorImage = image;
            }
        }

        public void ClearError()
        {
            IsError = false;
            ErrorMessage = "";
            ErrorImage = "";
        }
    }
}
