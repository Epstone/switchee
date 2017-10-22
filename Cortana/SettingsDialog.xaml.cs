using Reactive.Bindings;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CortanaHomeAutomation.MainApp
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;

    public sealed partial class SettingsDialog
    {
        public ReactiveProperty<string> GatewayIPAddress = new ReactiveProperty<string>();
        public ReactiveProperty<string> GatewayPort = new ReactiveProperty<string>();

        public SettingsDialog(string gatewayIpAddress, int gatewayPort)
        {
            this.InitializeComponent();

            this.GatewayIPAddress.Value = string.IsNullOrEmpty(gatewayIpAddress) ? string.Empty : gatewayIpAddress;
            this.GatewayPort.Value = gatewayPort == 0 ? "49880" : gatewayPort.ToString();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void Autodetect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Listen().ContinueWith(x=>Send()).Wait();
            }
            catch (Exception ex)
            {
                Debug.Write(ex); // todo open messagebox and print error
            }
        }

        private DatagramSocket listenerSocket;

        private async Task Listen()
        {
            if (listenerSocket == null)
            {
                listenerSocket = new DatagramSocket();
                listenerSocket.MessageReceived += MessageReceived;
                await listenerSocket.BindServiceNameAsync("123411");
            }
        }

        private async Task Send()
        {
            var localIp = IpAddressExtensions.GetLocalIp(HostNameType.Ipv4);
            var broadastAddress = IpAddressExtensions.GetBroadastAddress(localIp);

            IOutputStream outputStream = await listenerSocket.GetOutputStreamAsync(new HostName(broadastAddress.ToString()),"49880");

            using (DataWriter writer = new DataWriter(outputStream))
            {
                writer.WriteString("SEARCH HCGW");
                await writer.StoreAsync();
            }
        }

        async void MessageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs args)
        {
            //todo
            //using (DataReader reader = args.GetDataReader())
            //{
                //uint len = reader.UnconsumedBufferLength;
                //string msg = reader.ReadString(len);
                //string[] responseData = msg.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            //}

            this.GatewayIPAddress.Value = args.RemoteAddress.DisplayName;
        }
    }
}
