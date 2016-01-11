using System;

namespace CortanaHomeAutomation.MainApp
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
        public ELRO(String masterdip, String slavedip) : base(masterdip, slavedip)
        {
            
        }

        public override string CreateCommand(bool deviceState)
        {
            int sA = 0;
            int sG = 0;
            int sRepeat = 10;
            int sPause = 5600;
            int sTune = 350;
            String sBaud = "25";
            int sSpeed = 14;
            String HEAD = "TXP:" + sA + "," + sG + "," + sRepeat + "," + sPause + "," + sTune + "," + sBaud + ",";
            String TAIL = "1," + sSpeed + ",;";
            String AN = "1,3,1,3,1,3,3,1,";
            String AUS = "1,3,3,1,1,3,1,3,";
            int bitLow = 1;
            int bitHgh = 3;
            String seqLow = bitLow + "," + bitHgh + "," + bitLow + "," + bitHgh + ",";
            String seqHgh = bitLow + "," + bitHgh + "," + bitHgh + "," + bitLow + ",";
            String bits = this.Masterdip;
            String msg = "";

            for (int i = 0; i < bits.ToString().Length; i++)
            {
                String bit = bits.Substring(i, 1);
                if (bit == "1")
                {
                    msg = msg + seqLow;
                }
                else {
                    msg = msg + seqHgh;
                }
            }

            String msgM = msg;
            bits = this.Slavedip;
            msg = "";
            for (int i = 0; i < bits.ToString().Length; i++)
            {
                String bit = bits.Substring(i, 1);
                if (bit == "1")
                {
                    msg = msg + seqLow;
                }
                else {
                    msg = msg + seqHgh;
                }
            }
            String msgS = msg;

            string result;
            if (deviceState)
            {
                result = HEAD + msgM + msgS + AN + TAIL;
            }
            else {
                result = HEAD + msgM + msgS + AUS + TAIL;
            }

            return result;
        }

    }


}
