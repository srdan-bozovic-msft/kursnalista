using System;
using MSC.Universal.Shared.UI.Implementation;

namespace MSC.Universal.Shared.UI.Contracts.Views
{
    public interface IPageView
    {

    }

    public interface IPageView<out T> : IPageView
    {
        T ViewModel { get; }

        //IDictionary<string, object> State { get; }
    }
}
