using CortanaHomeAutomation.MainApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CortanaHomeAutomation.MainApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AppState _state;

        public bool ShowDeletionButtons { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._state = (AppState)e.Parameter;

            this.lv_Devices.ItemsSource = _state.Devices;

            base.OnNavigatedTo(e);
        }

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size { Height = 550, Width = 500 };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private static ObservableCollection<Device> CreateDevices()
        {
            var devices = new ObservableCollection<Device>();
            for (int j = 1; j <= 16; j++)
            {
                var device = new Intertechno("A", j.ToString());
                devices.Add(device);
            }
            return devices;
        }


        private string _gatewayIP = "192.168.0.156";
        private int _gatewayPort = 49880;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Intertechno device = new Intertechno("A", "3");
            ExecuteCommand(device, true);
        }

        private void ExecuteCommand(Device device, bool action)
        {
            string onCommand = device.CreateCommand(action);

            GatewayClient.Send(onCommand, _gatewayIP, _gatewayPort);
        }

        private async void btn_add_Click(object sender, RoutedEventArgs e)
        {
            AddDeviceDialog dialog = new AddDeviceDialog(false);
            ContentDialogResult dialogResult = await dialog.ShowAsync();

            if (dialogResult == ContentDialogResult.Primary)
            {
                this._state.Devices.Add(dialog.ViewModel.Device);
                await Startup.StoreAppState(_state);
            }
            else
            {
                // User pressed Cancel or the back arrow.
            }
        }
        
        private void btn_off_OnClick(object sender, RoutedEventArgs e)
        {
            this.lv_Devices.SelectedIndex = -1;
            var device = GetDeviceFromEvent(e);
            ExecuteCommand(device, false);
        }

        private void btn_on_OnClick(object sender, RoutedEventArgs e)
        {
            this.lv_Devices.SelectedIndex = -1;
            var device = GetDeviceFromEvent(e);
            ExecuteCommand(device, true);
        }


        private void btn_deleteDevice_OnClick(object sender, RoutedEventArgs e)
        {
            var device = GetSelectedDevice();
            this._state.Devices.Remove(device);
        }

        private Device GetDeviceFromEvent(RoutedEventArgs e)
        {
            var btn = e.OriginalSource as Button;
            if (btn != null)
            {
                var device = btn.DataContext as Device;
                return device;
            }
            return null;
        }

        private async void btn_editDevice_Click(object sender, RoutedEventArgs e)
        {
            AddDeviceDialog dialog = new AddDeviceDialog(true);
            var selectedDevice = GetSelectedDevice();
            var clonedDeviceForEdits = selectedDevice.CloneJson();
            dialog.ViewModel.Device = clonedDeviceForEdits;

            ContentDialogResult dialogResult = await dialog.ShowAsync();

            if (dialogResult == ContentDialogResult.Primary)
            {
                var index = this._state.Devices.IndexOf(selectedDevice);
                this._state.Devices.Remove(selectedDevice);
                this._state.Devices.Insert(index, clonedDeviceForEdits);

                await Startup.StoreAppState(_state);
            }
            else
            {
                // User pressed Cancel or the back arrow, so do nothing
            }
        }

        private Intertechno GetSelectedDevice()
        {
            return this.lv_Devices.SelectedItem as Intertechno;
        }

        private async void btn_saveConfig_Click(object sender, RoutedEventArgs e)
        {
            await Startup.SaveAppStateToUserDefinedLocation(this._state);
        }

        private async void btn_loadConfig_Click(object sender, RoutedEventArgs e)
        {
            var state = await Startup.LoadAppStateFromUserDefinedLocation();

        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public class SampleDeviceData : ObservableCollection<Device>
    {
        public SampleDeviceData()
        {
            this.Add(new Intertechno()
            {
                UserDefinedName = "Schlafzimmer Fenster",
                Masterdip = "A",
                Slavedip = "5"
            });

            this.Add(new Intertechno()
            {
                UserDefinedName = "Wohnzimmerbeleuchtung",
                Masterdip = "A",
                Slavedip = "7"
            });

            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });
            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });
            this.Add(new Intertechno()
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

        }
    }
}
