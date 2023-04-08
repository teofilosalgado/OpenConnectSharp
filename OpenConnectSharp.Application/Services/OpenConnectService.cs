using OpenConnectSharp.Application.Interfaces;
using System.Diagnostics;
using OpenConnectSharp.Domain.Models;
using OpenConnectSharp.Domain.Enums;
using Microsoft.Win32;

namespace OpenConnectSharp.Application.Services
{
    public class OpenConnectService : IOpenConnectService
    {
        private Process? process;
        private MainWindowForm? mainWindowFormCache;

        private Connection connection = Connection.Disconnected;
        private bool shouldRestartOnResume = false;

        public event EventHandler? Connected;
        public event EventHandler<int>? Disconnected;

        public OpenConnectService()
        {
            if (!OperatingSystem.IsWindows())
                return;
            SystemEvents.PowerModeChanged += OnPowerChanged;
        }

        private void OnProcessExited(object? sender, EventArgs e)
        {
            if (process is null || Disconnected is null)
                return;

            int exitCode = process.ExitCode;
            this.connection = Connection.Disconnected;
            Disconnected.Invoke(this, exitCode);
        }

        public void Toggle(MainWindowForm credentials)
        {
            if(this.connection == Connection.Disconnected)
            {
                this.Start(credentials);
            }
            else
            {
                this.Stop();
            }
        }

        public void Start(MainWindowForm credentials)
        {
            this.mainWindowFormCache = credentials;
            this.process = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    FileName = "openconnect.exe",
                    Arguments = $"--protocol=anyconnect --user={credentials.Username} --authgroup={credentials.Group} --useragent=AnyConnect --dump {credentials.Gateway}",
                }
            };
            process.Exited += OnProcessExited;
            process.Start();

            Connected?.Invoke(this, EventArgs.Empty);

            process.StandardInput.AutoFlush = true;
            process.StandardInput.WriteLine("yes");
            process.StandardInput.WriteLine(credentials.Password);
            process.StandardInput.Close();

            process.WaitForExitAsync();

            this.connection = Connection.Connected;
        }

        public void Stop()
        {
            process?.Kill();
            this.connection = Connection.Disconnected;
        }

        private void OnResumed()
        {
            Trace.WriteLine("OnResumed");
            if (this.shouldRestartOnResume && this.mainWindowFormCache is not null)
            {
                this.Start(this.mainWindowFormCache);
            }
        }

        private void OnSuspended()
        {
            Trace.WriteLine("OnSuspended");
            if (this.connection == Connection.Connected)
            {
                this.Stop();
                shouldRestartOnResume = true;
            }
        }

        private void OnPowerChanged(object s, PowerModeChangedEventArgs e)
        {
            if (!OperatingSystem.IsWindows())
                return;
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    this.OnResumed();
                    break;
                case PowerModes.Suspend:
                    this.OnSuspended();
                    break;
            }
        }
    }
}
