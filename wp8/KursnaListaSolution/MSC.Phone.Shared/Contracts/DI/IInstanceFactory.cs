using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC.Phone.Shared.Contracts.DI
{
    public interface IInstanceFactory
    {
        void RegisterType<TInterface, TService>()
            where TService : class, TInterface
            where TInterface : class;

        void RegisterInstance<TService>(TService instance)
            where TService : class;

        void RegisterNamedInstance<TService>(TService instance, string key)
            where TService : class;

        TService GetInstance<TService>();

        TService GetNamedInstance<TService>(string key);
    }
}
