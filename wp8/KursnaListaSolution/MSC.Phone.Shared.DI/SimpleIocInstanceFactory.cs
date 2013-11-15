using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MSC.Phone.Shared.Contracts.DI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MSC.Phone.Shared.DI
{
    public class SimpleIocInstanceFactory : IInstanceFactory
    {
        public static IInstanceFactory Default{get; private set;}

        static SimpleIocInstanceFactory()
        {
            Default = new SimpleIocInstanceFactory();
        }

        protected SimpleIocInstanceFactory()
        {
            SimpleIoc.Default.Register<IInstanceFactory>(() => this);
            ServiceLocator.SetLocatorProvider(() => this);
        }

        public void RegisterType<TInterface, TService>()
            where TService : class, TInterface
            where TInterface : class
        {
            //SimpleIoc.Default.Register<TInterface, TService>();
        }

        public void RegisterInstance<TService>(TService instance)
            where TService : class
        {
            SimpleIoc.Default.Register<TService>(() => instance);
        }

        public void RegisterNamedInstance<TService>(TService instance, string key)
            where TService : class
        {
            SimpleIoc.Default.Register<TService>(() => instance, key);
        }

        public TService GetInstance<TService>()
        {
            return SimpleIoc.Default.GetInstance<TService>();
        }

        public T GetNamedInstance<T>(string key)
        {
            return SimpleIoc.Default.GetInstance<T>(key);
        }


        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return SimpleIoc.Default.GetAllInstances<TService>();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return SimpleIoc.Default.GetAllInstances(serviceType);
        }

        public TService GetInstance<TService>(string key)
        {
            throw new NotImplementedException();
        }

        public object GetInstance(Type serviceType, string key)
        {
            return SimpleIoc.Default.GetInstance(serviceType, key);
        }

        public object GetInstance(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
