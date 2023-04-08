using OpenConnectSharp.Application.Interfaces;
using System.Diagnostics;
using OpenConnectSharp.Domain.Models;

namespace OpenConnectSharp.Application.Services
{
    public class OpenConnectService : IOpenConnectService
    {
        private Process? process;

        public event EventHandler<int>? ProcessExited;

        private void OnProcessExited(object? sender, EventArgs e)
        {
            if (process is null || ProcessExited is null)
                return;

            int exitCode = process.ExitCode;
            ProcessExited.Invoke(this, exitCode);
        }

        public void Start(Form credentials)
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

            process.StandardInput.AutoFlush = true;
            process.StandardInput.WriteLine("yes");
            process.StandardInput.WriteLine(credentials.Password);
            process.StandardInput.Close();

            process.WaitForExitAsync();
        }

        public void Stop()
        {
            process?.Kill();
        }
    }
}
