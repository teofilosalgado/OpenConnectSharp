using Caliburn.Micro;
using OpenConnectSharp.UI.ViewModels;
using System.Collections.Generic;
using System;
using System.Dynamic;
using System.Windows;
using OpenConnectSharp.Application.Interfaces;
using OpenConnectSharp.Application.Services;
using Serilog;
using OpenConnectSharp.UI.Hooks;

namespace OpenConnectSharp.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer container = new SimpleContainer();
        public static CaptureLogFilePathHook CaptureLogFilePathHook = new CaptureLogFilePathHook();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log_.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, hooks: CaptureLogFilePathHook)
                .CreateLogger();

            Log.Debug("Displaying main window asynchronously");
            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.NoResize;
            DisplayRootViewForAsync<MainWindowViewModel>(settings);
        }

        protected override void Configure()
        {
            // Services
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IOpenConnectService, OpenConnectService>();

            // Views
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
