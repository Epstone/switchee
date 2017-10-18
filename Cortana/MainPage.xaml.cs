namespace CortanaHomeAutomation.MainApp
{
    using System;
    using System.Collections.ObjectModel;
    using System.Net;
    using Windows.ApplicationModel.Resources;
    using Windows.Foundation;
    using Windows.UI.Popups;
    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using Utility;

    public sealed partial class MainPage
    {
        private AppState currentAppState;
        private readonly ResourceLoader stringRessource = new ResourceLoader();

        public MainPage()
        {
            InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size {Height = 550, Width = 500};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        public bool ShowDeletionButtons { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var state = (AppState) e.Parameter;
            UpdateViewFromState(state);
            base.OnNavigatedTo(e);
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

        private void ExecuteCommand(Device device, bool action)
        {
            IPAddress outValue;
            if (!IPAddress.TryParse(currentAppState.GatewayIPAddress, out outValue))
            {
                ShowUserWarning(stringRessource.GetString("WarningIncorrectIPAddress"));
                return;
            }

            try
            {
                string onCommand = device.CreateCommand(action);
                GatewayClient.Send(onCommand, currentAppState.GatewayIPAddress, currentAppState.GatewayPort);
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
                currentAppState.Devices.Add(dialog.ViewModel.Device);
                await Startup.StoreAppState(currentAppState);
            }
        }

        private void btn_off_OnClick(object sender, RoutedEventArgs e)
        {
            lv_Devices.SelectedIndex = -1;
            var device = GetDeviceFromEvent(e);
            ExecuteCommand(device, false);
        }

        private void btn_on_OnClick(object sender, RoutedEventArgs e)
        {
            lv_Devices.SelectedIndex = -1;
            var device = GetDeviceFromEvent(e);
            ExecuteCommand(device, true);
        }


        private void btn_deleteDevice_OnClick(object sender, RoutedEventArgs e)
        {
            var device = GetSelectedDevice();
            currentAppState.Devices.Remove(device);
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
                var index = currentAppState.Devices.IndexOf(selectedDevice);
                currentAppState.Devices.Remove(selectedDevice);
                currentAppState.Devices.Insert(index, clonedDeviceForEdits);

                await Startup.StoreAppState(currentAppState);
            }
        }

        private Intertechno GetSelectedDevice()
        {
            return lv_Devices.SelectedItem as Intertechno;
        }

        private async void btn_saveConfig_Click(object sender, RoutedEventArgs e)
        {
            await Startup.SaveAppStateToUserDefinedLocation(currentAppState);
        }

        private async void btn_loadConfig_Click(object sender, RoutedEventArgs e)
        {
            var state = await Startup.LoadAppStateFromUserDefinedLocation();
            UpdateViewFromState(state);
        }

        private void UpdateViewFromState(AppState state)
        {
            currentAppState = state;
            lv_Devices.ItemsSource = currentAppState.Devices;

            if (string.IsNullOrEmpty(currentAppState.GatewayIPAddress))
            {
                ShowSettingsDialog();
            }
        }

        private void ShowSettingsDialog()
        {
            btn_settings_Click(this, null);
        }

        private async void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsDialog settingsDialog = new SettingsDialog(currentAppState.GatewayIPAddress, currentAppState.GatewayPort);
            ContentDialogResult dialogResult = await settingsDialog.ShowAsync();

            if (dialogResult == ContentDialogResult.Primary)
            {
                currentAppState.GatewayIPAddress = settingsDialog.GatewayIPAddress.Value;
                currentAppState.GatewayPort = int.Parse(settingsDialog.GatewayPort.Value);
                await Startup.StoreAppState(currentAppState);
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
            Add(new Intertechno
            {
                UserDefinedName = "Schlafzimmer Fenster",
                Masterdip = "A",
                Slavedip = "5"
            });

            Add(new Intertechno
            {
                UserDefinedName = "Wohnzimmerbeleuchtung",
                Masterdip = "A",
                Slavedip = "7"
            });

            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });
            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });

            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });
            Add(new Intertechno
            {
                UserDefinedName = "Rollo Küche",
                Masterdip = "A",
                Slavedip = "12"
            });
        }
    }
}