using Caliburn.Micro;
using Mapster;
using OpenConnectSharp.Application.Interfaces;
using OpenConnectSharp.Domain.Enums;
using OpenConnectSharp.Domain.Models;
using System.Diagnostics;
using System.Windows;

namespace OpenConnectSharp.UI.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly IWindowManager windowManager;
        private readonly IOpenConnectService openConnectService;

        public string StatusLabelColor { get; set; } = "#FFB71C1C";
        public string StatusIconPath { get; set; } = "/Resources/LockOpen.png";
        public string ActionButtonLabel { get; set; } = "Connect";

        public Form Form { get; set; } = new Form();
        public Status Status { get; set; } = new Status();

        private void UpdateUserSettings()
        {
            if (Form.SaveCredentials)
            {
                this.Form.Adapt(Properties.Settings.Default);
            }
            else
            {
                Properties.Settings.Default.Reset();
            }
            Properties.Settings.Default.SaveCredentials = this.Form.SaveCredentials;
            Properties.Settings.Default.Save();
        }

        private void ReadUserSettings()
        {
            Properties.Settings.Default.Adapt(this.Form);
        }

        private void OnProcessExited(object? sender, int exitCode)
        {
            if (exitCode == 1)
            {
                MessageBox.Show("Something went wrong. Check your credentials or network connection.", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnConnected(object? sender, Connection connection)
        {
            openConnectService.Start(this.Form);
        }

        private void OnDisconnected(object? sender, Connection connection)
        {
            openConnectService.Stop();
        }

        public void OnClickActionButton()
        {
            this.UpdateUserSettings();
            this.Status.Toggle();
            NotifyOfPropertyChange(nameof(Status));
        }

        public void OnClickViewLog()
        {

        }

        public MainWindowViewModel(IWindowManager windowManager, IOpenConnectService openConnectService)
        {
            this.windowManager = windowManager;
            this.openConnectService = openConnectService;
            this.openConnectService.ProcessExited += OnProcessExited;

            this.Status.Connected += OnConnected;
            this.Status.Disconnected += OnDisconnected;

            this.DisplayName = "OpenConnectSharp";

            this.ReadUserSettings();
        }
    }
}
