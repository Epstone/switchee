using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
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
    public sealed partial class AddDeviceDialog : ContentDialog
    {
        internal AddDialogViewModel ViewModel { get; set; }
        public bool EditMode { get; internal set; }

        public AddDeviceDialog(bool editMode)
        {
            this.EditMode = editMode;

            this.InitializeComponent();
            this.ViewModel =  new AddDialogViewModel();

            this.cmbxMasterSwitch.ItemsSource = ViewModel.MasterSwitches;
            this.cmbxSlaveSwitch.ItemsSource = ViewModel.SlaveSwitches;

            if (EditMode)
            {
                this.PrimaryButtonText = "Übernehmen";
            }
            else
            {
                this.SecondaryButtonText = "Abbrechen";
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
