using System;

namespace MSC.Universal.Shared.Contracts.DeviceServices
{
    public interface IToastService
    {
        void Show(string title, string content);
        void Show(string title, string content, Uri uri);
    }
}
