using System;
using System.Collections.Generic;
using System.Linq;

namespace CortanaHomeAutomation.GatewayControl
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Objekt erzeugen
            ELRO device = new ELRO("10101", "10000", "192.168.2.112", 49880);

            //Einschalten
            device.on();

            //Ausschalten
            device.off();
        }
    }
}
