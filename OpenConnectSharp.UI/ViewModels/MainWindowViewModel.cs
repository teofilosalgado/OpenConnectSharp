using Caliburn.Micro;
using Mapster;
using OpenConnectSharp.Application.Interfaces;
using OpenConnectSharp.Domain.Models;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

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
                this.MainWindowForm.Adapt(Properties.Settings.Default);
            }
            else
            {
                Properties.Settings.Default.Reset();
            }
            Properties.Settings.Default.SaveCredentials = this.MainWindowForm.SaveCredentials;
            Properties.Settings.Default.Save();
        }

        private void ReadUserSettings()
        {
            Properties.Settings.Default.Adapt(this.MainWindowForm);
        }

        private void OnConnected(object? sender, EventArgs e)
        {
            MainWindowStatus.Connect();
            NotifyOfPropertyChange(nameof(MainWindowStatus));
        }

        private void OnDisconnected(object? sender, int exitCode)
        {
            if (exitCode == 1)
            {
                MessageBox.Show("Something went wrong. Check your credentials or network connection.", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MainWindowStatus.Disconnect();
            NotifyOfPropertyChange(nameof(MainWindowStatus));
        }

        public override Task<bool> CanCloseAsync(CancellationToken cancellationToken = default)
        {
            this.openConnectService.Stop();
            return base.CanCloseAsync(cancellationToken);
        }

        public void OnClickActionButton()
        {
            this.UpdateUserSettings();
            this.openConnectService.Toggle(this.MainWindowForm);
        }

        public void OnClickViewLog()
        {
            // TODO
        }

        public MainWindowViewModel(IOpenConnectService openConnectService)
        {
            this.openConnectService = openConnectService;
            this.openConnectService.Connected += OnConnected;
            this.openConnectService.Disconnected += OnDisconnected;

            this.DisplayName = "OpenConnectSharp";

            this.ReadUserSettings();
        }
    }
}
