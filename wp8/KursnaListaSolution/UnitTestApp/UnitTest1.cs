using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using KursnaLista.Phone.ViewModels;
using UnitTestApp.Mocks.Repositories;
using KursnaLista.Phone.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace UnitTestApp
{
    [TestClass]
    public class ConverterPageViewModelUnitTest
    {
        [TestMethod]
        public void TestCalculation()
        {
            var kursnaListaZaDan = new KursnaListaZaDan()
            {
                SrednjiKurs = new List<StavkaKursneListe>
                {
                    new StavkaKursneListe(){ VaziZa = 1, SrednjiKurs = 100, OznakaValute="EUR"},
                    new StavkaKursneListe(){ VaziZa = 1, SrednjiKurs = 50, OznakaValute="USD"},
                }
            };
            var repo = new MockKursnaListaRepository(kursnaListaZaDan);
            var pageViewModel = new ConverterPageViewModel(repo, null);

            var waitHandle = new AutoResetEvent(false);

            Deployment.Current.Dispatcher.BeginInvoke(
                async () =>
                {
                    await pageViewModel.LoadData("EUR", "USD");
                    waitHandle.Set();
                });

            waitHandle.WaitOne(TimeSpan.FromSeconds(5000));

            pageViewModel.Iznos = "1000";
            pageViewModel.KonvertujCommand.Execute(null);
            Assert.AreEqual(2000, pageViewModel.Result);

        }
    }
}
