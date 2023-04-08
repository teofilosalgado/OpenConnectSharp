using OpenConnectSharp.Domain.Enums;

namespace OpenConnectSharp.Domain.Models
{
    public class Status
    {
        public string LabelForeground { get; private set; } = string.Empty;
        public string LabelContent { get; private set; } = string.Empty;
        public string IconSource { get; private set; } = string.Empty;
        public string ActionButtonContent { get; private set; } = string.Empty;
        public Connection Connection { get; private set; } = Connection.Disconnected;

        public event EventHandler<Connection>? Connected;

        public event EventHandler<Connection>? Disconnected;

        public Status()
        {
            this.Disconnect();
        }

        public void Toggle()
        {
            if (Connection == Connection.Connected)
            {
                this.Disconnect();
            }
            else
            {
                this.Connect();
            }
        }

        private void Connect()
        {
            this.LabelForeground = "#FF1B5E20";
            this.LabelContent = "Connected";
            this.IconSource = "/Resources/LockClosed.png";
            this.ActionButtonContent = "Disconnect";
            this.Connection = Connection.Connected;
            Connected?.Invoke(this, Connection);
        }

        private void Disconnect()
        {
            this.LabelForeground = "#FFB71C1C";
            this.LabelContent = "Disconnected";
            this.IconSource = "/Resources/LockOpen.png";
            this.ActionButtonContent = "Connect";
            this.Connection = Connection.Disconnected;
            Disconnected?.Invoke(this, Connection);
        }
    }
}
