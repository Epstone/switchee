using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using lavalampe;

namespace CortanaHomeAutomation.GatewayControl
{
    /// <summary>
    /// Die Schalter 1 bis 5 sind der masterdip und die Schalter A bis E sind der slavedip.
    /// </summary>
    public class ELRO : Device
    {
        /// <summary>
        /// Die Schalter 1 bis 5 sind der masterdip und die Schalter A bis E sind der slavedip.
        /// </summary>
        /// <param name="masterdip">Der Masterdip</param>
        /// <param name="slavedip">Der Slavedip</param>
        /// <param name="ip">IP Adresse des Connair</param>
        public ELRO(String masterdip, String slavedip, String ip) : base(masterdip, slavedip, ip)
        {

        }

        /// <summary>
        /// Die Schalter 1 bis 5 sind der masterdip und die Schalter A bis E sind der slavedip.
        /// </summary>
        /// <param name="masterdip">Der Masterdip</param>
        /// <param name="slavedip">Der Slavedip</param>
        /// <param name="ip">IP Adresse des Connair</param>
        /// <param name="port">Der Port des Connair</param>
        public ELRO(String masterdip, String slavedip, String ip, int port) : base(masterdip, slavedip, ip, port)
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
            int sRepeat = 10;
            int sPause = 5600;
            int sTune = 350;
            String sBaud = "25";
            int sSpeed = 14;
            String HEAD = "TXP:"+sA+","+sG+","+sRepeat+","+sPause+","+sTune+","+sBaud+",";
            String TAIL = "1,"+sSpeed+",;";
            String AN = "1,3,1,3,1,3,3,1,";
            String AUS = "1,3,3,1,1,3,1,3,";
            int bitLow = 1;
            int bitHgh = 3;
            String seqLow = bitLow + "," + bitHgh + "," + bitLow + "," + bitHgh + ",";
            String seqHgh = bitLow + "," + bitHgh + "," + bitHgh + "," + bitLow + ",";
            String bits = this.masterdip;
            String msg = "";

            for(int i=0;i < bits.ToString().Length; i++) {   
                String bit = bits.Substring(i,1);
                if(bit=="1") {
                    msg = msg+seqLow;
                } else {
                    msg = msg+seqHgh;
                }
            }

            String msgM = msg;
            bits = this.slavedip;
            msg = "";
            for(int i=0;i < bits.ToString().Length; i++) {    
                String bit = bits.Substring(i,1);
                if(bit=="1") {
                    msg = msg+seqLow;
                } else {
                    msg = msg+seqHgh;
                }
            }
            String msgS = msg;
            if(action == "ON") {
                return HEAD+msgM+msgS+AN+TAIL;
            } else {
                return HEAD+msgM+msgS+AUS+TAIL;
            }
        }
    }

  
}
