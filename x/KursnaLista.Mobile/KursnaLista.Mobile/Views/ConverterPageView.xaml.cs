using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using KursnaLista.Phone.Contracts.Views;
using KursnaLista.Phone.ViewModels;
using MSC.Phone.Shared.Contracts.ViewModels;

namespace KursnaLista.Mobile.Views
{
    public partial class ConverterPageView : IConverterPageView
    {
        public ConverterPageView()
        {
            InitializeComponent();
            BindingContext = App.Locator.ConverterPageViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.Navigation = Navigation;
            await ViewModel.InitializeAsync();

            var vm = ViewModel as ConverterPageViewModel;
            if (vm != null)
            {
                if (PickerValutaIz.Items.Count == 0)
                {
                    foreach (var item in vm.ValutaIzItems)
                    {
                        PickerValutaIz.Items.Add(item.Naziv);
                    }
                }

                if (PickerValutaU.Items.Count == 0)
                {
                    foreach (var item in vm.ValutaUItems)
                    {
                        PickerValutaU.Items.Add(item.Naziv);
                    }
                }
            }
        }

        public IPageViewModel ViewModel
        {
            get { return BindingContext as IPageViewModel; }
        }
    }
}
