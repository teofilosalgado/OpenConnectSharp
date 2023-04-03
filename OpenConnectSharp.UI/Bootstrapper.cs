using Caliburn.Micro;
using OpenConnectSharp.UI.ViewModels;
using System.Collections.Generic;
using System;
using System.Dynamic;
using System.Windows;

namespace OpenConnectSharp.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.NoResize;
            DisplayRootViewForAsync<MainWindowViewModel>(settings);
        }

        protected override void Configure()
        {
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<MainWindowViewModel>();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}
