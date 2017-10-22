using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CortanaHomeAutomation.MainApp
{
    using System.Diagnostics;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Networking;
    using Windows.Networking.Connectivity;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;

    public sealed partial class SettingsDialog : ContentDialog
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
                Debug.Write(ex);
                //Handle exception here.            
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
            string localIp = GetLocalIp(HostNameType.Ipv4);
            var ipAddress = IPAddress.Parse(localIp);
            var broadastAddress = GetBroadastAddress(ipAddress);

            IOutputStream outputStream = await listenerSocket.GetOutputStreamAsync(new HostName(broadastAddress.ToString()),"49880");

            using (DataWriter writer = new DataWriter(outputStream))
            {
                writer.WriteString("SEARCH HCGW");
                string result = IPAddress.Broadcast.ToString();
                await writer.StoreAsync();
            }
        }

        public static IPAddress GetSubnetMask(IPAddress hostAddress)
        {
            var addressBytes = hostAddress.GetAddressBytes();
            if (addressBytes[0] >= 1 && addressBytes[0] <= 126)
                return IPAddress.Parse("255.0.0.0");
            else if (addressBytes[0] >= 128 && addressBytes[0] <= 191)
                return IPAddress.Parse("255.255.255.0");
            else if (addressBytes[0] >= 192 && addressBytes[0] <= 223)
                return IPAddress.Parse("255.255.255.0");
            else
                throw new ArgumentOutOfRangeException();
        }

        //This method is the one that will give the Directed broadcast address:
        public static IPAddress GetBroadastAddress(IPAddress hostIPAddress)
        {
            var subnetAddress = GetSubnetMask(hostIPAddress);
            var deviceAddressBytes = hostIPAddress.GetAddressBytes();
            var subnetAddressBytes = subnetAddress.GetAddressBytes();
            if (deviceAddressBytes.Length != subnetAddressBytes.Length)
                throw new ArgumentOutOfRangeException();
            var broadcastAddressBytes = new byte[deviceAddressBytes.Length];
            for (var i = 0; i < broadcastAddressBytes.Length; i++)
                broadcastAddressBytes[i] = (byte)(deviceAddressBytes[i] | subnetAddressBytes[i] ^ 255);
            return new IPAddress(broadcastAddressBytes);
        }

        public static string GetLocalIp(HostNameType hostNameType = HostNameType.Ipv4)
        {
            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

            if (internetConnectionProfile?.NetworkAdapter == null) return null;
            var hostname =
                NetworkInformation.GetHostNames()
                    .FirstOrDefault(
                        hostName =>
                            hostName.Type == hostNameType &&
                            hostName.IPInformation?.NetworkAdapter != null &&
                            hostName.IPInformation.NetworkAdapter.NetworkAdapterId == internetConnectionProfile.NetworkAdapter.NetworkAdapterId);

            // the ip address
            return hostname?.CanonicalName;
        }

        async void MessageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs args)
        {
            DataReader reader = args.GetDataReader();
            uint len = reader.UnconsumedBufferLength;
            string msg = reader.ReadString(len);

            string[] responseData = msg.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            string remoteHost = args.RemoteAddress.DisplayName;
            reader.Dispose();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.tbx_gatewayAddress.Text = remoteHost;
            });

        }
    }
}
