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
    }
}
