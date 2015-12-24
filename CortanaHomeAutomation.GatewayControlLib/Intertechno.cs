using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using lavalampe;

namespace CortanaHomeAutomation.GatewayControl
{
    /// <summary>
    /// Die Schalter 1 bis 5 sind der masterdip und die Schalter A bis E sind der slavedip.
    /// </summary>
    public class Intertechno : Device
    {
        /// <summary>
        /// Die Schalter 1 bis 5 sind der masterdip und die Schalter A bis E sind der slavedip.
        /// </summary>
        /// <param name="masterdip">Der Masterdip</param>
        /// <param name="slavedip">Der Slavedip</param>
        /// <param name="ip">IP Adresse des Connair</param>
        public Intertechno(String masterdip, String slavedip, String ip) : base(masterdip, slavedip, ip)
        {

        }

        /// <summary>
        /// Die Schalter 1 bis 5 sind der masterdip und die Schalter A bis E sind der slavedip.
        /// </summary>
        /// <param name="masterdip">Der Masterdip</param>
        /// <param name="slavedip">Der Slavedip</param>
        /// <param name="ip">IP Adresse des Connair</param>
        /// <param name="port">Der Port des Connair</param>
        public Intertechno(String masterdip, String slavedip, String ip, int port) : base(masterdip, slavedip, ip, port)
        {

        }

        /// <summary>
        /// Baut die Nachricht zusammen, welche an den Connair gesendet wird.
        /// </summary>
        /// <param name="action">ON oder OFF</param>
        /// <returns>Die Nachricht für das Connair</returns>
        protected override String _createMsg(String action)
        {
            int sA = 0;
            int sG = 0;
            int sRepeat = 12;
            int sPause = 11125;
            int sTune = 89;
            int sBaud = 25;
            int sSpeed = 32; //erfahrung aus dem Forum auf 32 stellen http://forum.power-switch.eu/viewtopic.php?f=15&t=146
            int uSleep = 800000;
            string HEAD = "TXP:" + sA + "," + sG + "," + sRepeat + "," + sPause + "," + sTune + "," + sBaud + ",";
            String TAIL = ",1," + sSpeed + ",;";
            string AN = "12,4,4,12,12,4";
            string AUS = "12,4,4,12,4,12";
            int bitLow = 4;
            int bitHgh = 12;
            string seqLow = bitHgh + "," + bitHgh + "," + bitLow + "," + bitLow + ",";
            string seqHgh = bitHgh + "," + bitLow + "," + bitHgh + "," + bitLow + ",";
            string msgM = "";
            switch ( this.masterdip.ToUpperInvariant())
            {
                case "A":
                    msgM = seqHgh + seqHgh + seqHgh + seqHgh;
                    break;
                case "B":
                    msgM = seqLow + seqHgh + seqHgh + seqHgh;
                    break;
                case "C":
                    msgM = seqHgh + seqLow + seqHgh + seqHgh;
                    break;
                case "D":
                    msgM = seqLow + seqLow + seqHgh + seqHgh;
                    break;
                case "E":
                    msgM = seqHgh + seqHgh + seqLow + seqHgh;
                    break;
                case "F":
                    msgM = seqLow + seqHgh + seqLow + seqHgh;
                    break;
                case "G":
                    msgM = seqHgh + seqLow + seqLow + seqHgh;
                    break;
                case "H":
                    msgM = seqLow + seqLow + seqLow + seqHgh;
                    break;
                case "I":
                    msgM = seqHgh + seqHgh + seqHgh + seqLow;
                    break;
                case "J":
                    msgM = seqLow + seqHgh + seqHgh + seqLow;
                    break;
                case "K":
                    msgM = seqHgh + seqLow + seqHgh + seqLow;
                    break;
                case "L":
                    msgM = seqLow + seqLow + seqHgh + seqLow;
                    break;
                case "M":
                    msgM = seqHgh + seqHgh + seqLow + seqLow;
                    break;
                case "N":
                    msgM = seqLow + seqHgh + seqLow + seqLow;
                    break;
                case "O":
                    msgM = seqHgh + seqLow + seqLow + seqLow;
                    break;
                case "P":
                    msgM = seqLow + seqLow + seqLow + seqLow;
                    break;
            }
            string msgS = "";
            switch (this.slavedip)
            {
                case "1":
                    msgS = seqHgh + seqHgh + seqHgh + seqHgh;
                    break;
                case "2":
                    msgS = seqLow + seqHgh + seqHgh + seqHgh;
                    break;
                case "3":
                    msgS = seqHgh + seqLow + seqHgh + seqHgh;
                    break;
                case "4":
                    msgS = seqLow + seqLow + seqHgh + seqHgh;
                    break;
                case "5":
                    msgS = seqHgh + seqHgh + seqLow + seqHgh;
                    break;
                case "6":
                    msgS = seqLow + seqHgh + seqLow + seqHgh;
                    break;
                case "7":
                    msgS = seqHgh + seqLow + seqLow + seqHgh;
                    break;
                case "8":
                    msgS = seqLow + seqLow + seqLow + seqHgh;
                    break;
                case "9":
                    msgS = seqHgh + seqHgh + seqHgh + seqLow;
                    break;
                case "10":
                    msgS = seqLow + seqHgh + seqHgh + seqLow;
                    break;
                case "11":
                    msgS = seqHgh + seqLow + seqHgh + seqLow;
                    break;
                case "12":
                    msgS = seqLow + seqLow + seqHgh + seqLow;
                    break;
                case "13":
                    msgS = seqHgh + seqHgh + seqLow + seqLow;
                    break;
                case "14":
                    msgS = seqLow + seqHgh + seqLow + seqLow;
                    break;
                case "15":
                    msgS = seqHgh + seqLow + seqLow + seqLow;
                    break;
                case "16":
                    msgS = seqLow + seqLow + seqLow + seqLow;
                    break;
            }
            if (action == "ON")
            {
                return HEAD + bitLow + "," + msgM + msgS + seqHgh + seqLow + bitHgh + "," + AN + TAIL;
            }
            else {
                return HEAD + bitLow + "," + msgM + msgS + seqHgh + seqLow + bitHgh + "," + AUS + TAIL;
            }
        }
    }


}
