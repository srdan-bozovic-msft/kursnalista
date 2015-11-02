namespace MSC.Universal.Shared.UI.Contracts.ViewModels
{
    public interface IViewItem
    {
        bool IsNavigable { get; }

        bool IsVisible { get; }
    }
}