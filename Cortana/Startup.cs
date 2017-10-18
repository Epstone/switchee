using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace CortanaHomeAutomation.MainApp
{
    using Windows.Storage.Pickers;

    public class Startup
    {
        private const string _settingsFilename = "HomeAutomationSettings.xml";
        private const string _userDefinedSettingsToken = "SettingsFileToken";

        private static ApplicationDataContainer LocalSettings { get { return ApplicationData.Current.LocalSettings; } }

        public static async Task<AppState> LoadAppStateInitial()
        {
            var file = await TryLoadConfigFile().ConfigureAwait(false);

            AppState resultState = new AppState();

            // app state no yet stored
            if (file == null)
            {
                await StoreInitialAppState(resultState);
            }
            else
            {
                try
                {
                    resultState = await XMLStorage.ReadObjectFromXmlFileAsync<AppState>(file).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // try to overwrite inconsistent state or crash
                    await StoreInitialAppState(resultState);
                }
            }

            return resultState;
        }

        private static async Task StoreInitialAppState(AppState appState)
        {
            await XMLStorage.SaveObjectToXmlByFileName(appState, _settingsFilename).ConfigureAwait(false);
        }

        private static async Task<StorageFile> TryLoadConfigFile()
        {
            StorageFile file = null;
            string token = string.Empty;
            
            if (LocalSettings.Values.ContainsKey(_userDefinedSettingsToken))
            {
                token = LocalSettings.Values[_userDefinedSettingsToken] as string;
            }

            if (!string.IsNullOrEmpty(token) && StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
            {
                try
                {
                    file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);
                }
                catch (FileNotFoundException ex)
                {
                    // file no longer existing, remove entry
                    StorageApplicationPermissions.FutureAccessList.Remove(token);
                    LocalSettings.Values.Remove(_userDefinedSettingsToken);
                }
            }
            else
            {
                file = (StorageFile)await ApplicationData.Current.LocalFolder.TryGetItemAsync(_settingsFilename);
            }
            return file;
        }

        public static async Task StoreAppState(AppState state)
        {
            var configFile = await TryLoadConfigFile();
            await XMLStorage.SaveObjectToXmlByFile(state, configFile);
        }

        internal static async Task SaveAppStateToUserDefinedLocation(AppState appState)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".xml" });
            savePicker.SuggestedFileName = "HomeAutomationSettings";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                var token = StorageApplicationPermissions.FutureAccessList.Add(file, _userDefinedSettingsToken);
                LocalSettings.Values[_userDefinedSettingsToken] = token;

                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                //Windows.Storage.CachedFileManager.DeferUpdates(file);  crashes when used with dropbox
                // write to file
                await XMLStorage.SaveObjectToXmlByFile(appState, file);
                
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
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
                Debug.WriteLine("Saving file was cancelled.");
            }
        }

        internal static async Task<AppState> LoadAppStateFromUserDefinedLocation()
        {
            AppState resultState = null;

            var loadConfigPicker = new FileOpenPicker();
            loadConfigPicker.ViewMode = PickerViewMode.List;
            loadConfigPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            loadConfigPicker.FileTypeFilter.Add(".xml");


            StorageFile file = await loadConfigPicker.PickSingleFileAsync();
            if (file != null)
            {
                var token = StorageApplicationPermissions.FutureAccessList.Add(file, _userDefinedSettingsToken);
                LocalSettings.Values[_userDefinedSettingsToken] = token;

                resultState = await XMLStorage.ReadObjectFromXmlFileAsync<AppState>(file);
            }
            else
            {
                Debug.WriteLine("Loading file was cancelled.");
            }

            return resultState;
        }
    }
}
