using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace CortanaHomeAutomation.MainApp
{
    [XmlInclude(typeof(Device))]
    [XmlInclude(typeof(Intertechno))]
    public class AppState
    {
        public AppState()
        {
            this.Devices = new ObservableCollection<Device>();
        }
        public ObservableCollection<Device> Devices { get; set; }
    }
}
