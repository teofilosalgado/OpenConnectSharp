using OpenConnectSharp.Domain.Enums;

namespace OpenConnectSharp.Domain.Models
{
    public class MainWindowStatus
    {
        public string LabelForeground { get; private set; } = string.Empty;
        public string LabelContent { get; private set; } = string.Empty;
        public string IconSource { get; private set; } = string.Empty;
        public string ActionButtonContent { get; private set; } = string.Empty;
        public bool IsFormEnabled { get; private set; } = true;

        public MainWindowStatus()
        {
            this.Disconnect();
        }

        public void Connect()
        {
            this.LabelForeground = "#FF1B5E20";
            this.LabelContent = "Connected";
            this.IconSource = "/Resources/LockClosed.png";
            this.ActionButtonContent = "Disconnect";
            this.IsFormEnabled = false;
        }

        public void Disconnect()
        {
            this.LabelForeground = "#FFB71C1C";
            this.LabelContent = "Disconnected";
            this.IconSource = "/Resources/LockOpen.png";
            this.ActionButtonContent = "Connect";
            this.IsFormEnabled = true;
        }
    }
}
