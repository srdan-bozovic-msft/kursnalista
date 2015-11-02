using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using MSC.Universal.Shared.Contracts.DI;

namespace MSC.Universal.Shared.DI
{
    public class SimpleIocInstanceFactory : IInstanceFactory
    {
        public static IInstanceFactory Default { get; private set; }

        public Dictionary<Type, Type> Registrations =
            new Dictionary<Type, Type>();

        static SimpleIocInstanceFactory()
        {
            Default = new SimpleIocInstanceFactory();
        }

        protected SimpleIocInstanceFactory()
        {
            SimpleIoc.Default.Register<IInstanceFactory>(() => this);
        }

        public void RegisterType<TInterface, TService>()
            where TService : class, TInterface
            where TInterface : class
        {
            Registrations.Add(typeof(TInterface), typeof(TService));
            SimpleIoc.Default.Register<TInterface, TService>();
        }

        public void RegisterType<TService>()
            where TService : class
        {
            SimpleIoc.Default.Register<TService>(true);
        }

        public void RegisterInstance<TService>(TService instance)
            where TService : class
        {
            SimpleIoc.Default.Register(() => instance);
        }

        public void RegisterNamedInstance<TService>(TService instance, string key)
            where TService : class
        {
            SimpleIoc.Default.Register(() => instance, key);
        }

        public TInterface GetInstance<TInterface>()
        {
            return SimpleIoc.Default.GetInstance<TInterface>();
        }

        public Type GetType<TInterface>()
        {
            return Registrations[typeof(TInterface)];
        }

        public T GetNamedInstance<T>(string key)
        {
            return SimpleIoc.Default.GetInstance<T>(key);
        }
    }
}
