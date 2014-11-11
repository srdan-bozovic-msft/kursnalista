using System;

namespace MSC.Universal.Shared.Contracts.DI
{
    public interface IInstanceFactory
    {
        void RegisterType<TInterface, TService>()
            where TService : class, TInterface
            where TInterface : class;

        void RegisterType<TService>()
            where TService : class;

        void RegisterInstance<TService>(TService instance)
            where TService : class;

        void RegisterNamedInstance<TService>(TService instance, string key)
            where TService : class;

        TService GetInstance<TService>();

        Type GetType<TInterface>();

        TService GetNamedInstance<TService>(string key);
    }
}
