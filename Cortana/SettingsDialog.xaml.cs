namespace CortanaHomeAutomation.MainApp
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Reactive.Bindings;

    public sealed partial class SettingsDialog : IDisposable
    {
        public ReactiveProperty<string> GatewayIPAddress = new ReactiveProperty<string>();
        public ReactiveProperty<string> GatewayPort = new ReactiveProperty<string>();

        private DatagramSocket listenerSocket;

        public SettingsDialog(string gatewayIpAddress, int gatewayPort)
        {
            InitializeComponent();

            GatewayIPAddress.Value = string.IsNullOrEmpty(gatewayIpAddress) ? string.Empty : gatewayIpAddress;
            GatewayPort.Value = gatewayPort == 0 ? "49880" : gatewayPort.ToString();
        }

        public void Dispose()
        {
            GatewayIPAddress?.Dispose();
            GatewayPort?.Dispose();
            listenerSocket?.Dispose();
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
                Listen().ContinueWith(x => Send()).Wait();
            }
            catch (Exception ex)
            {
                Debug.Write(ex); // todo open messagebox and print error
            }
        }

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

            IOutputStream outputStream = await listenerSocket.GetOutputStreamAsync(new HostName(broadastAddress.ToString()), "49880");

            using (DataWriter writer = new DataWriter(outputStream))
            {
                writer.WriteString("SEARCH HCGW");
                await writer.StoreAsync();
            }
        }

        private async void MessageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs args)
        {
            //todo
            //using (DataReader reader = args.GetDataReader())
            //{
            //uint len = reader.UnconsumedBufferLength;
            //string msg = reader.ReadString(len);
            //string[] responseData = msg.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            //}

            GatewayIPAddress.Value = args.RemoteAddress.DisplayName;
        }
    }
}