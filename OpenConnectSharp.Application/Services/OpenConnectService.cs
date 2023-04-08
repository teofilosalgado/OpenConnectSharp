using OpenConnectSharp.Application.Interfaces;
using System.Diagnostics;
using OpenConnectSharp.Domain.Models;
using OpenConnectSharp.Domain.Enums;

namespace OpenConnectSharp.Application.Services
{
    public class OpenConnectService : IOpenConnectService
    {
        private Process? process;
        private Connection connection;

        public event EventHandler? Connected;
        public event EventHandler<int>? Disconnected;

        public OpenConnectService()
        {
            this.connection = Connection.Disconnected;
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
    }
}
