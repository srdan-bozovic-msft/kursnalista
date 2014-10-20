using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using KursnaLista.Phone.Contracts.ViewModels;
using KursnaLista.Phone.Contracts.Views;
using KursnaLista.Phone.ViewModels;
using MSC.Phone.Shared.Contracts.ViewModels;
using Xamarin.Forms;

namespace KursnaLista.Mobile.Views
{
    public partial class ConverterPageView : IConverterPageView
    {
        public ConverterPageView()
        {
            InitializeComponent();
            BindingContext = App.Locator.ConverterPageViewModel;

            if (Device.OS == TargetPlatform.WinPhone)
            {
                ToolbarItem toolbarItem = null;

                toolbarItem = new ToolbarItem("konvertuj", "exchange.png", () =>
                {
                    var converterPageViewModel = ViewModel as IConverterPageViewModel;
                    converterPageViewModel.SetTileCommand.Execute(null);
                    SetToolbarItem(toolbarItem, converterPageViewModel);
                });

                SetToolbarItem(toolbarItem, App.Locator.ConverterPageViewModel);

                ToolbarItems.Add(toolbarItem);
            }
        }

        private void SetToolbarItem(ToolbarItem toolbarItem, IConverterPageViewModel converterPageViewModel)
        {
            toolbarItem.Name = converterPageViewModel.SetTileButtonText;
            toolbarItem.Icon = new FileImageSource(){ File = converterPageViewModel.SetTileButtonIconUri.ToString()};
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
