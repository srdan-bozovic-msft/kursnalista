using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MSC.Universal.Shared.Contracts.Repositories;
using MSC.Universal.Shared.Contracts.Services;

namespace MSC.Universal.Shared.Implementation
{
    public abstract class RepositoryBase
    {
        private readonly ICacheService _cacheService;

        protected ICacheService CacheService
        {
            get { return _cacheService; }
        }

        protected RepositoryBase(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public static string ToCacheKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            //http://stackoverflow.com/questions/3009284/using-regex-to-replace-invalid-characters
            const string pattern = "[\\~#%&*{}/:<>?|\"-]";
            var regEx = new Regex(pattern);
            return Regex.Replace(regEx.Replace(key, " "), @"\s+", "_");
        }

        protected Task<RepositoryResult<T>> LoadFromCacheAsync<T>(
            string key,
            TimeSpan staleAfter,
            TimeSpan expireAfter,
            Func<Task<ServiceResult<T>>> getDataFuncAsync
            )
        {
            return LoadFromCacheAsync(key, null, null, staleAfter, expireAfter, getDataFuncAsync);
        }

        protected async Task<RepositoryResult<T>> LoadFromCacheAsync<T>(
            string key,
            DateTime? updatedTime,
            Func<T, DateTime> updatedTimeFunc,
            TimeSpan staleAfter,
            TimeSpan expireAfter,
            Func<Task<ServiceResult<T>>> getDataFuncAsync
            )
        {
            try
            {
                var useUpdatedTime = updatedTime != null;
                var cacheItem = useUpdatedTime
                    ? await _cacheService.GetAsync<T>(key, updatedTime.Value).ConfigureAwait(false)
                    : await _cacheService.GetAsync<T>(key).ConfigureAwait(false);

                if (!cacheItem.HasValue ||
                    cacheItem.IsExpired)
                {
                    var serviceResult = await getDataFuncAsync().ConfigureAwait(false);
                    if (serviceResult.Successful)
                    {
                        if (useUpdatedTime)
                        {
                            // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
                                _cacheService.PutAsync(key, updatedTimeFunc(serviceResult.Value), serviceResult.Value,
                                    DateTime.Now + staleAfter, DateTime.Now + expireAfter);
                        }
                        else
                        {
                            // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
                            _cacheService.PutAsync(key, serviceResult.Value,
                                DateTime.Now + staleAfter, DateTime.Now + expireAfter);
                        }
                    }
                    return serviceResult;
                }
                return RepositoryResult<T>.Create(cacheItem.Value, !cacheItem.IsStale);
            }
            catch (Exception xcp)
            {
                return RepositoryResult<T>.CreateError(xcp);
            }
        }

        protected Task<RepositoryResult<T>> LoadFallbackToCacheAsync<T>(
            string key,
            TimeSpan staleAfter,
            TimeSpan expireAfter,
            Func<Task<ServiceResult<T>>> getDataFuncAsync
            )
        {
            return LoadFallbackToCacheAsync(key, null, null, staleAfter, expireAfter, getDataFuncAsync);
        }

        protected async Task<RepositoryResult<T>> LoadFallbackToCacheAsync<T>(
            string key,
            DateTime? updatedTime,
            Func<T, DateTime> updatedTimeFunc,
            TimeSpan staleAfter,
            TimeSpan expireAfter,
            Func<Task<ServiceResult<T>>> getDataFuncAsync
            )
        {
            try
            {
                var useUpdatedTime = updatedTime != null;

                var serviceResult = await getDataFuncAsync().ConfigureAwait(false);
                if (serviceResult.Successful)
                {
                    if (useUpdatedTime)
                    {
                        // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
                        _cacheService.PutAsync(key, updatedTimeFunc(serviceResult.Value), serviceResult.Value,
                            DateTime.Now + staleAfter, DateTime.Now + expireAfter);
                    }
                    else
                    {
                        // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
                        _cacheService.PutAsync(key, serviceResult.Value,
                            DateTime.Now + staleAfter, DateTime.Now + expireAfter);
                    }
                    return serviceResult;
                }

                var cacheItem = useUpdatedTime
                    ? await _cacheService.GetAsync<T>(key, updatedTime.Value).ConfigureAwait(false)
                    : await _cacheService.GetAsync<T>(key).ConfigureAwait(false);

                if (!cacheItem.HasValue ||
                    cacheItem.IsExpired)
                {
                    return RepositoryResult<T>.CreateError(new KeyNotFoundException());
                }
                return RepositoryResult<T>.Create(cacheItem.Value, !cacheItem.IsStale);
            }
            catch (Exception xcp)
            {
                return RepositoryResult<T>.CreateError(xcp);
            }
        }

        protected Task<RepositoryResult<T>> LoadFromCacheThenRefreshAsync<T>(
            string key,
            TimeSpan staleAfter,
            TimeSpan expireAfter,
            Func<Task<ServiceResult<T>>> getDataFuncAsync,
            Func<RepositoryResult<T>, Task> refreshActionAsync = null
            )
        {
            return LoadFromCacheThenRefreshAsync(key, null, null, staleAfter, expireAfter, getDataFuncAsync, refreshActionAsync);
        }

        protected async Task<RepositoryResult<T>> LoadFromCacheThenRefreshAsync<T>(
            string key,
            DateTime? updatedTime,
            Func<T, DateTime> updatedTimeFunc,
            TimeSpan staleAfter,
            TimeSpan expireAfter,
            Func<Task<ServiceResult<T>>> getDataFuncAsync,
            Func<RepositoryResult<T>, Task> refreshActionAsync = null
            )
        {
            try
            {
                var useUpdatedTime = updatedTime != null;
                var cacheItem = useUpdatedTime
                    ? await _cacheService.GetAsync<T>(key, updatedTime.Value)
                    : await _cacheService.GetAsync<T>(key);

                if (cacheItem.HasValue &&
                    !cacheItem.IsStale &&
                    !cacheItem.IsExpired)
                {
                    return RepositoryResult<T>.Create(cacheItem.Value);
                }

                if (cacheItem.HasValue &&
                    cacheItem.IsStale &&
                    !cacheItem.IsExpired &&
                    refreshActionAsync != null)
                {
                    refreshActionAsync(RepositoryResult<T>.Create(cacheItem.Value, false));
                }

                var serviceResult = await getDataFuncAsync().ConfigureAwait(false);
                if (serviceResult.Successful)
                {
                    if (useUpdatedTime)
                    {
                        // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
                        _cacheService.PutAsync(key, updatedTimeFunc(serviceResult.Value), serviceResult.Value,
                            DateTime.Now + staleAfter, DateTime.Now + expireAfter);
                    }
                    else
                    {
                        // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
                        _cacheService.PutAsync(key, serviceResult.Value,
                            DateTime.Now + staleAfter, DateTime.Now + expireAfter);
                    }
                }
                return serviceResult;
            }
            catch (Exception xcp)
            {
                return RepositoryResult<T>.CreateError(xcp);
            }
        }

        protected Task<RepositoryResult<T>> AppendToCacheAsync<T>(
            string key,
            Func<Task<ServiceResult<T>>> getDataAsync,
            Func<T, T, T> updateDataFunc,
            Func<T, T, T> filterDataFunc
            )
        {
            return AppendToCacheAsync(key, null, getDataAsync, updateDataFunc, filterDataFunc);
        }


        protected async Task<RepositoryResult<T>> AppendToCacheAsync<T>(
            string key,
            DateTime? updatedTime,
            Func<Task<ServiceResult<T>>> getDataFuncAsync,
            Func<T, T, T> updateDataFunc,
            Func<T, T, T> filterDataFunc
            )
        {
            try
            {
                var serviceResult = await getDataFuncAsync().ConfigureAwait(false);
                if (serviceResult.Successful)
                {
                    var useUpdatedTime = updatedTime != null;
                    var cacheItem = useUpdatedTime
                        ? await _cacheService.GetAsync<T>(key, updatedTime.Value).ConfigureAwait(false)
                        : await _cacheService.GetAsync<T>(key).ConfigureAwait(false);
                    if (cacheItem.HasValue)
                    {
                        var filteredItems = filterDataFunc(cacheItem.Value, serviceResult.Value);
                        if (useUpdatedTime)
                        {
                            // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
                            _cacheService.UpdateAsync(key, updatedTime.Value, updateDataFunc(cacheItem.Value, serviceResult.Value));
                        }
                        else
                        {
                            // ReSharper disable once CSharpWarnings::CS4014, no need to wait storage operation
                            _cacheService.UpdateAsync(key, updateDataFunc(cacheItem.Value, serviceResult.Value));
                        }
                        return filteredItems;
                    }
                }
                return serviceResult;
            }
            catch (Exception xcp)
            {
                return RepositoryResult<T>.CreateError(xcp);
            }
        }
    }
}
