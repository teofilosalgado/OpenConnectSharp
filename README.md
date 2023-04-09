# OpenConnectSharp

A windows-only alternative client for Cisco's AnyConnect VPN protocol based on [OpenConnect](https://www.infradead.org/openconnect/).

# Features

- An easy drop-in solution for OpenConnect GUI Windows users running exclusively on Cisco's AnyConnect protocol. This should prevent issues with user agent validation related [here](https://gitlab.com/openconnect/openconnect-gui/-/issues/331) and [here](https://gitlab.com/openconnect/openconnect-gui/-/issues/271).
- Modern and simple .NET 7 based WPF application with a relatively low memory footprint.
- Automatic management of Windows power events preventing disconnections whenever the computer sleeps or hibernates.

# Instalation

1. Clone this repository and build the solution.
2. Install OpenConnect from the [official repository](https://www.infradead.org/openconnect/) . Don't forget to add it to the PATH.
3. Copy [this script](https://github.com/teofilosalgado/OpenConnectSharp/blob/master/OpenConnectSharp.UI/Resources/vpnc-script-win.js) to your OpenConnect installation folder root (usually `C:\Program Files\OpenConnect`). Replace it if necessary.
4. Run the built application.

# Screenshots

![application screenshot while connected](static/connected.png)
![application screenshot while disconnected](static/disconnected.png)

# To Do

- [ ] Create an installer bundling the OpenConnect executable and script.
- [ ] Design CI/CD pipelines using Github workflows.
- [ ] Add support for other protocols.