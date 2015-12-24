using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Threading;
using System.Threading.Tasks;
using GatewayLib;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CortanaHomeAutomation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.lv_Devices.ItemsSource = new ObservableCollection<string>() {"Hello 12", "Hello13"};
        }

        


        private string _gatewayIP = "192.168.0.156";
        private int _gatewayPort = 49880;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Intertechno device = new Intertechno("A", "3");
            string onCommand = device.CreateCommand(true);

            GatewayClient.Send(onCommand, _gatewayIP, _gatewayPort);
        }

        private void btn_off_Click(object sender, RoutedEventArgs e)
        {
            Intertechno device = new Intertechno("A", "3");
            string onCommand = device.CreateCommand(false);

            GatewayClient.Send(onCommand, _gatewayIP, _gatewayPort);
        }

        private int i = 0;
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            lv_Devices.Items.Add("Hello world + i");
            i++;
        }
    }
}
