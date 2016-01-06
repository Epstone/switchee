using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CortanaHomeAutomation
{
    [XmlInclude(typeof(Device))]
    [XmlInclude(typeof(Intertechno))]
    public class AppState
    {
        public AppState()
        {
            
        }
        public ObservableCollection<Device> Devices { get; set; }
    }
}
