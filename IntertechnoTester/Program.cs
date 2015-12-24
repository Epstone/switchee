using CortanaHomeAutomation.GatewayControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntertechnoTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Intertechno device = new Intertechno("A", "3", "192.168.0.156", 49880);
            device.on();
        }
    }
}
