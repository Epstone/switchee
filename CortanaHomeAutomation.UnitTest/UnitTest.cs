using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CortanaHomeAutomation.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Save_And_Load_AsXml()
        {
            AppState state = new AppState();

            state.Devices = new ObservableCollection<Device>();
            state.Devices.Add(new Intertechno("A", "3"));
            state.Devices.Add(new Intertechno("A", "4"));
            state.Devices.Add(new Intertechno("A", "5"));

            string filename = "testdata_settings.xml";

            await XMLStorage.SaveObjectToXml(state, filename);


            var result = await XMLStorage.ReadObjectFromXmlFileAsync<AppState>(filename);

            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            var text = await Windows.Storage.FileIO.ReadTextAsync(file);

            Assert.AreEqual(3, result.Devices.Count);
            Assert.AreEqual("A", result.Devices.FirstOrDefault().Masterdip);
            Assert.AreEqual("3", result.Devices.FirstOrDefault().Slavedip);
        }
    }
}
