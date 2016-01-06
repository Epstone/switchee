using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CortanaHomeAutomation.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Save_And_Load_AsXml()
        {
            ObservableCollection<Device> devices = new ObservableCollection<Device>();
            devices.Add(new Intertechno("A","3"));
            devices.Add(new Intertechno("A", "4"));
            devices.Add(new Intertechno("A", "5"));


        }
    }
}
