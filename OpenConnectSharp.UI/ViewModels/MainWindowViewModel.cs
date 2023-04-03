using Caliburn.Micro;
using System.Dynamic;
using System.Windows;

namespace OpenConnectSharp.UI.ViewModels
{
	public class MainWindowViewModel : Screen
    {
        private readonly IWindowManager windowManager;

        private bool status = false;

		public bool Status
		{
			get { return status; }
			set { 
				if(!status)
				{
					StatusLabelColor = "#FF1B5E20";
                    StatusIconPath = "/Resources/LockClosed.png";
                    IsIndeterminate = true;
                } else
				{
					StatusLabelColor = "#FFB71C1C";
					StatusIconPath = "/Resources/LockOpen.png";
                    IsIndeterminate = false;
                }
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(StatusLabelColor));
                NotifyOfPropertyChange(nameof(StatusIconPath));
                NotifyOfPropertyChange(nameof(IsIndeterminate));
                status = value;
            }
		}

		public string StatusLabelColor { get; set; } = "#FFB71C1C";
		public string StatusIconPath { get; set; } = "/Resources/LockOpen.png";
        public bool IsIndeterminate { get; set; } = false;

        public string Gateway { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        //public void OnClickNewProfile()
        //{
        //          dynamic settings = new ExpandoObject();
        //          settings.WindowStyle = WindowStyle.ToolWindow;
        //          settings.ShowInTaskbar = false;
        //          windowManager.ShowWindowAsync(new CreateProfileViewModel(), null, settings);
        //}

        public void OnClickConnect()
		{
			this.Status = !this.Status;
		}

        public void OnClickViewLog()
        {

        }

        public MainWindowViewModel(IWindowManager windowManager)
		{
			this.windowManager = windowManager;
            this.DisplayName = "OpenConnectSharp";
        }
    }
}
