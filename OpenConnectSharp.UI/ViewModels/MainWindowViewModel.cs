using Caliburn.Micro;
using Mapster;
using OpenConnectSharp.Application.Interfaces;
using OpenConnectSharp.Domain.Models;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Serilog;

namespace OpenConnectSharp.UI.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly IOpenConnectService openConnectService;

        public MainWindowForm MainWindowForm { get; set; } = new MainWindowForm();
        public MainWindowStatus MainWindowStatus { get; set; } = new MainWindowStatus();

        private void UpdateUserSettings()
        {
            if (MainWindowForm.SaveCredentials)
            {
                Log.Debug("Updating user settings");
                this.MainWindowForm.Adapt(Properties.Settings.Default);
            }
            else
            {
                Log.Debug("Resetting user settings");
                Properties.Settings.Default.Reset();
            }
            Log.Debug("Writing user settings");
            Properties.Settings.Default.SaveCredentials = this.MainWindowForm.SaveCredentials;
            Properties.Settings.Default.Save();
        }

        private void ReadUserSettings()
        {
            Log.Debug("Reading user settings");
            Properties.Settings.Default.Adapt(this.MainWindowForm);
        }

        private void OnConnected(object? sender, EventArgs e)
        {
            Log.Debug("Application is connected");
            MainWindowStatus.Connect();
            NotifyOfPropertyChange(nameof(MainWindowStatus));
        }

        private void OnDisconnected(object? sender, int exitCode)
        {
            Log.Debug($"Application was disconnected with exit code: {exitCode}");
            if (exitCode == 1)
            {
                MessageBox.Show("Something went wrong. Check your credentials or network connection.", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MainWindowStatus.Disconnect();
            NotifyOfPropertyChange(nameof(MainWindowStatus));
        }

        public override Task<bool> CanCloseAsync(CancellationToken cancellationToken = default)
        {
            Log.Debug("Closing application");
            this.openConnectService.Stop();
            return base.CanCloseAsync(cancellationToken);
        }

        public void OnClickActionButton()
        {
            Log.Debug("Action button clicked");
            this.UpdateUserSettings();
            this.openConnectService.Toggle(this.MainWindowForm);
        }

        public void OnClickViewLog()
        {
            // TODO: open log file with file viewer
        }

        public MainWindowViewModel(IOpenConnectService openConnectService)
        {
            Log.Debug("Connecting service events to methods");
            this.openConnectService = openConnectService;
            this.openConnectService.Connected += OnConnected;
            this.openConnectService.Disconnected += OnDisconnected;

            this.DisplayName = "OpenConnectSharp";

            this.ReadUserSettings();
        }
    }
}
