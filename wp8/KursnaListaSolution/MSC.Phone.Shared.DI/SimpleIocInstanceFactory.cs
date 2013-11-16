using GalaSoft.MvvmLight.Ioc;
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
        }

        public void RegisterType<TInterface, TService>()
            where TService : class, TInterface
            where TInterface : class
        {
            SimpleIoc.Default.Register<TInterface, TService>();
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
    }
}
