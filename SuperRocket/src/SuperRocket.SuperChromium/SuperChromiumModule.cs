﻿using System;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using SuperRocket.SuperChromium.Services;

namespace SuperRocket.SuperChromium
{
    public class SuperChromiumModule : IModule
    {
        private readonly IUnityContainer _container;
        
        public SuperChromiumModule(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException($"{nameof(container)}");
            }

            _container = container;
        }

        public void Initialize()
        {
            //BrowserManager manager = new BrowserManager();
            //_container.RegisterInstance(typeof(IBrowserManager), manager);
            _container.RegisterType<IBrowserManager, BrowserManager>(new ContainerControlledLifetimeManager());
        }
    }
}