using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace CortanaHomeAutomation
{
    /// <summary>
    /// Basisklasse für alle Geräte
    /// </summary>
    [XmlInclude(typeof(Device))]
    public abstract class Device
    {
        /// <summary>
        /// Der Masterdip
        /// </summary>
        public string Masterdip { get; set; }

        /// <summary>
        /// Der Slavedip
        /// </summary>
        public string Slavedip { get; set; }

        public Device()
        {
            
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="masterdip">Der Masterdip</param>
        /// <param name="slavedip">Der Slavedip</param>
        public Device(String masterdip, String slavedip)
        {
            this.Masterdip = masterdip;
            this.Slavedip = slavedip;
        }

        /// <summary>
        /// Schaltet das Gerät
        /// </summary>
        public abstract string CreateCommand(bool deviceState);
        
    }
}
