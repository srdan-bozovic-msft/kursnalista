using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace MSC.Universal.Shared.Implementation
{
    public class IncrementalObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        private readonly Func<Task<IList<T>>> _loadNextPage;

        public IncrementalObservableCollection(Func<Task<IList<T>>> loadNextPage)
        {
            _loadNextPage = loadNextPage;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return InnerLoadMoreItemsAsync(count).AsAsyncOperation();
        }

        private async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint expectedCount)
        {
            var actualCount = 0;
            IList<T> dataItems;

            try
            {
                dataItems = await _loadNextPage();
            }
            catch (Exception)
            {
                HasMoreItems = false;
                throw;
            }

            if (dataItems != null && dataItems.Any())
            {
                foreach (var item in dataItems)
                {
                    Add(item);
                }

                actualCount += dataItems.Count;
            }
            else
            {
                HasMoreItems = false;
            }

            return new LoadMoreItemsResult
            {
                Count = (uint)actualCount
            };
        }

        public bool HasMoreItems { get; private set; }
    }
}
