using Serilog.Sinks.File;
using System.IO;
using System.Text;


namespace OpenConnectSharp.UI.Hooks
{
    public class CaptureLogFilePathHook : FileLifecycleHooks
    {
        public string? Path { get; private set; }

        public override Stream OnFileOpened(string path, Stream underlyingStream, Encoding encoding)
        {
            this.Path = path;
            return base.OnFileOpened(path, underlyingStream, encoding);
        }
    }
}
