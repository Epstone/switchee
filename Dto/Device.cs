using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CortanaHomeAutomation
{
    /// <summary>
    /// Basisklasse für alle Geräte
    /// </summary>
    public abstract class Device
    {
        /// <summary>
        /// Der Masterdip
        /// </summary>
        protected string _masterdip;

        /// <summary>
        /// Der Slavedip
        /// </summary>
        protected string _slavedip;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="masterdip">Der Masterdip</param>
        /// <param name="slavedip">Der Slavedip</param>
        public Device(String masterdip, String slavedip)
        {
            this._masterdip = masterdip;
            this._slavedip = slavedip;
        }

        /// <summary>
        /// Schaltet das Gerät
        /// </summary>
        public abstract string CreateCommand(bool deviceState);
        
    }
}
