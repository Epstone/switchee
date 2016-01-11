using System;
using System.Xml.Serialization;

namespace CortanaHomeAutomation.MainApp
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

        public string UserDefinedName { get; set; }

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
