using System;
using System.Text;

namespace CortanaHomeAutomation
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
        public Intertechno(String masterdip, String slavedip) : base(masterdip, slavedip)
        {

        }

        /// <summary>
        /// Baut die Nachricht zusammen, welche an den Connair gesendet wird.
        /// </summary>
        /// <param name="action">ON oder OFF</param>
        /// <param name="deviceState">Target state for device e.g. on/off</param>
        /// <returns>Die Nachricht für das Connair</returns>
        public override string CreateCommand(bool deviceState)
        {
            string resultOld = CreateCommandOriginal(deviceState);
            string resultNew = CreateCommandWlanSwitch(deviceState);

            return resultNew;
        }

        private string CreateCommandWlanSwitch(bool deviceState)
        {
            int i = 0;
            int j = 3;

            bool flag1 = true;
            String[] array = new String[2];
            array[0] = "4,12,4,12,";
            array[1] = "4,12,12,4,";
            String s = encodeBits(array, (inverse4Bits(i) << 4) + inverse4Bits(j - 1), 8);
            String s1;
            if (deviceState)
            {
                i = ((flag1) ? 1 : 0);
            }
            else
            {
                i = 0;
            }
            s1 = array[i];
            return ((new StringBuilder()).Append("TXP:0,0,5,11125,89,25,").Append(s).Append("4,12,4,12,4,12,12,4,4,12,12,4,").Append(s1).Append("4,63").Append(";").ToString());
        }

        protected static String encodeBits(String[] array, int i, int j)
        {
            String s = "";
            for (int k = 0; k < j; k++)
            {
                s = (new StringBuilder()).Append(s).Append(array[i >> j - 1 - k & 1]).ToString();
            }

            return s;
        }

        protected static int inverse4Bits(int i)
        {
            return (i & 1) << 3 | (i & 2) << 1 | (i & 4) >> 1 | i >> 3 & 1;
        }

        private string CreateCommandOriginal(bool deviceState)
        {
            int sA = 0;
            int sG = 0;
            int sRepeat = 5;
            int sPause = 11125;
            int sTune = 89;
            int sBaud = 25;
            int sSpeed = 63; //erfahrung aus dem Forum auf 32 stellen http://forum.power-switch.eu/viewtopic.php?f=15&t=146
            //int uSleep = 800000;
            string HEAD = "TXP:" + sA + "," + sG + "," + sRepeat + "," + sPause + "," + sTune + "," + sBaud + ",";
            String TAIL = ",4," + sSpeed + ";";
            string AN = "12,4,4,12,12,4";
            string AUS = "12,4,4,12,4,12";
            int bitLow = 4;
            int bitHgh = 12;
            string seqLow = bitHgh + "," + bitHgh + "," + bitLow + "," + bitLow + ",";
            string seqHgh = bitHgh + "," + bitLow + "," + bitHgh + "," + bitLow + ",";
            string msgM = "";
            switch (this._masterdip.ToUpperInvariant())
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
            switch (this._slavedip)
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

            string result = string.Empty;
            if (deviceState)
            {
                result = HEAD + bitLow + "," + msgM + msgS + seqHgh + seqLow + bitHgh + "," + AN + TAIL;
            }
            else
            {
                result = HEAD + bitLow + "," + msgM + msgS + seqHgh + seqLow + bitHgh + "," + AUS + TAIL;
            }

            return result;
        }
    }
}
