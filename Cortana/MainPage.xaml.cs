using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._state = (AppState)e.Parameter;

            this.lv_Devices.ItemsSource = _state.Devices;

            base.OnNavigatedTo(e);
        }

        public MainPage()
        {
            this.InitializeComponent();
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
            dlgAddDevice dialog = new dlgAddDevice();
            ContentDialogResult dialogResult = await dialog.ShowAsync();

            if (dialogResult == ContentDialogResult.Primary)
            {
                this._state.Devices.Add(dialog.ViewModel.Device);
                await Startup.StoreAppState(_state);
            }
            else
            {
                // User pressed Cancel or the back arrow.
                // Terms of use were not accepted.
            }

        }


        private async void btn_save_OnClick(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".xml" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "HomeAutomationSettings";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, file.Name);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Debug.WriteLine("File " + file.Name + " was saved.");
                }
                else
                {
                    Debug.WriteLine("File " + file.Name + " couldn't be saved.");
                }
            }
            else
            {
                Debug.WriteLine("Operation cancelled.");
            }

        }

        private void btn_off_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = e.OriginalSource as Button;
            if (btn != null)
            {
                var device = btn.DataContext as Device;
                ExecuteCommand(device, false);
            }
        }

        private void btn_on_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = e.OriginalSource as Button;
            if (btn != null)
            {
                var device = btn.DataContext as Device;
                ExecuteCommand(device, true);
            }
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

        }
    }
}
