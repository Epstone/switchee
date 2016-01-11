using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace CortanaHomeAutomation.MainApp
{
    public class Startup
    {
        private const string _settingsFilename = "HomeAutomationSettings.xml";
        private const string _userDefinedSettingsToken = "SettingsFileToken";

        public static async Task<AppState> LoadAppState()
        {
            var file = await TryLoadSettingsFile().ConfigureAwait(false);
            
            AppState resultState = null;

            // app state no yet stored
            if (file == null)
            {
                await StoreNewAppState();
            }
            else
            {
                try
                {
                    resultState = await XMLStorage.ReadObjectFromXmlFileAsync<AppState>(file).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await StoreNewAppState();
                }
            }

            return resultState;
        }

        private static async Task StoreNewAppState()
        {
            AppState resultState;
            resultState = new AppState();
            await XMLStorage.SaveObjectToXml(resultState, _settingsFilename).ConfigureAwait(false);
        }

        private static async Task<StorageFile> TryLoadSettingsFile()
        {
            StorageFile file = null;

            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(_userDefinedSettingsToken))
            {
                file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(_userDefinedSettingsToken);
            }
            else
            {
                file = (StorageFile)await ApplicationData.Current.LocalFolder.TryGetItemAsync(_settingsFilename);
            }
            return file;
        }

        public static async Task StoreAppState(AppState state)
        {
            await XMLStorage.SaveObjectToXml(state, _settingsFilename);
        }
    }
}
