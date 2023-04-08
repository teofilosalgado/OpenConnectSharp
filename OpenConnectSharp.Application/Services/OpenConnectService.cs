using OpenConnectSharp.Application.Interfaces;
using System.Diagnostics;
using OpenConnectSharp.Domain.Models;
using OpenConnectSharp.Domain.Enums;
using Microsoft.Win32;
using Serilog;

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

        }

        private void OnProcessExited(object? sender, EventArgs e)
        {
            Log.Debug("Process exited");
            if (process is null || Disconnected is null)
                return;

            int exitCode = process.ExitCode;
            this.connection = Connection.Disconnected;
            Disconnected.Invoke(this, exitCode);
        }

        public void Toggle(MainWindowForm credentials)
        {
            if (this.connection == Connection.Disconnected)
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
            Log.Debug("Starting openconnect.exe process");
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

            if (!OperatingSystem.IsWindows())
                return;
            SystemEvents.PowerModeChanged += OnPowerChanged;
        }

        public void Stop()
        {
            if (process is null)
                return;

            Log.Debug("Stopping process");
            this.connection = Connection.Disconnected;
            process.Kill();

            if (!OperatingSystem.IsWindows() || shouldRestartOnResume)
                return;
            SystemEvents.PowerModeChanged -= OnPowerChanged;
        }

        private void OnResumed()
        {
            Log.Debug("Power mode changed to: Resume");
            if (this.shouldRestartOnResume && this.mainWindowFormCache is not null)
            {
                Log.Debug("Reopening connection");
                this.Start(this.mainWindowFormCache);
                this.shouldRestartOnResume = false;
            }
        }

        private void OnSuspended()
        {
            Log.Debug("Power mode changed to: Suspend");
            if (this.connection == Connection.Connected)
            {
                Log.Debug("Scheduling connection to be reopened when resumed");
                shouldRestartOnResume = true;
                Log.Debug("Closing connection to the gateway");
                this.Stop();
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
