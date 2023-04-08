using OpenConnectSharp.Domain.Models;

namespace OpenConnectSharp.Application.Interfaces
{
    public interface IOpenConnectService
    {
        public event EventHandler? Connected;
        public event EventHandler<int>? Disconnected;

        public void Toggle(MainWindowForm form);
        public void Start(MainWindowForm credentials);
        public void Stop();
    }
}
