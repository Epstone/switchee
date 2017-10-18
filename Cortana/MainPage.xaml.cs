using CortanaHomeAutomation.MainApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Popups;
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
        ResourceLoader stringRessource = new Windows.ApplicationModel.Resources.ResourceLoader();
        
        public bool ShowDeletionButtons { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var state = (AppState)e.Parameter;
            this.UpdateViewFromState(state);
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Intertechno device = new Intertechno("A", "3");
            ExecuteCommand(device, true);
        }

        private void ExecuteCommand(Device device, bool action)
        {
            IPAddress outValue;
            if (!IPAddress.TryParse(_state.GatewayIPAddress, out outValue))
            {
                
                ShowUserWarning(stringRessource.GetString("WarningIncorrectIPAddress"));
                return;
            }

            try
            {
                string onCommand = device.CreateCommand(action);
                GatewayClient.Send(onCommand, _state.GatewayIPAddress, _state.GatewayPort);
            }
            catch (Exception)
            {
                ShowUserWarning(stringRessource.GetString("WarningCommandNotSend"));
            }
        }

        private async void ShowUserWarning(string message)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(message);

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("Close", null));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
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
            this.UpdateViewFromState(state);
        }

        private void UpdateViewFromState(AppState state)
        {
            this._state = state;
            this.lv_Devices.ItemsSource = _state.Devices;

            if (string.IsNullOrEmpty(_state.GatewayIPAddress))
            {
                this.btn_settings_Click(this, null);
            }
        }

        private async void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsDialog settingsDialog = new SettingsDialog(this._state.GatewayIPAddress, this._state.GatewayPort);
            ContentDialogResult dialogResult = await settingsDialog.ShowAsync();

            if (dialogResult == ContentDialogResult.Primary)
            {
                this._state.GatewayIPAddress = settingsDialog.GatewayIPAddress.Value;
                this._state.GatewayPort = int.Parse(settingsDialog.GatewayPort.Value);
                await Startup.StoreAppState(_state);
            }
            else
            {
                // User pressed Cancel or the back arrow.
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            await aboutDialog.ShowAsync();
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
