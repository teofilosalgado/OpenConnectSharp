using OpenConnectSharp.Domain.Models;

namespace OpenConnectSharp.Application.Interfaces
{
    public interface IOpenConnectService
    {
        public event EventHandler<int>? ProcessExited;
        public void Start(Form credentials);
        public void Stop();
    }
}
