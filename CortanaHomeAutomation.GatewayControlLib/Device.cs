using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lavalampe
{
    /// <summary>
    /// Basisklasse für alle Geräte
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Der Masterdip
        /// </summary>
        protected String masterdip = "";

        /// <summary>
        /// Der Slavedip
        /// </summary>
        protected String slavedip = "";

        /// <summary>
        /// IP Adresse des Connair
        /// </summary>
        protected String ip = "";

        /// <summary>
        /// Der Port des Connair
        /// </summary>
        protected int port = 49880;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="masterdip">Der Masterdip</param>
        /// <param name="slavedip">Der Slavedip</param>
        /// <param name="ip">IP Adresse des Connair</param>
        public Device(String masterdip, String slavedip, String ip)
        {
            this.masterdip = masterdip;
            this.slavedip = slavedip;
            this.ip = ip;
        }
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="masterdip">Der Masterdip</param>
        /// <param name="slavedip">Der Slavedip</param>
        /// <param name="ip">IP Adresse des Connair</param>
        /// <param name="port">Der Port des Connair</param>
        public Device(String masterdip, String slavedip, String ip, int port)
        {
            this.masterdip = masterdip;
            this.slavedip = slavedip;
            this.ip = ip;
            this.port = port;
        }

        /// <summary>
        /// Schaltet das Gerät ein
        /// </summary>
        public void on()
        {
            this.send(this._createMsg("ON"));
        }

        /// <summary>
        /// Schaltet das Gerät aus
        /// </summary>
        public void off()
        {
            this.send(this._createMsg("OFF"));
        }

        /// <summary>
        /// Baut die Nachricht zusammen, welche an den Connair gesendet wird. Muss in der abgeleiteten Klasse überschrieben werden.
        /// </summary>
        /// <param name="action">ON oder OFF</param>
        /// <returns>Die Nachricht für das Connair</returns>
        protected virtual String _createMsg(String action)
        {
            return "";
        }

        /// <summary>
        /// Sendet die Nachricht an das Connair
        /// </summary>
        /// <param name="text">Die Nachricht für das Connair</param>
        protected async Task send(String text)
        {
            String server = this.ip;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress address = IPAddress.Parse(server);
            IPEndPoint remoteEP = new IPEndPoint(address, this.port);
            AutoResetEvent semaphore = new AutoResetEvent(false);

            try
            {
                SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
                eventArgs.RemoteEndPoint = remoteEP;

                byte[] payload = Encoding.UTF8.GetBytes(text);
                eventArgs.SetBuffer(payload, 0, payload.Length);

                eventArgs.Completed += (object source, SocketAsyncEventArgs e) =>
                {
                    semaphore.Set();
                    Debug.WriteLine("Send Completed, error : " + e.SocketError + "Bytes transferred: " + e.BytesTransferred);
                };

                bool success = socket.SendToAsync(eventArgs);

                bool triggered = semaphore.WaitOne(TimeSpan.FromSeconds(2));

                if (!triggered)
                {
                    Debug.WriteLine("Semaphore NOT triggered");
                }
                else
               { 
                    Debug.WriteLine("Semaphore triggered");
                }

                if (!success)
                {
                    Debug.WriteLine("Send failed immediatly: " + eventArgs.SocketError);
                }
                else
                {
                    Debug.WriteLine("Send succeeded: " + eventArgs.SocketError);
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                {
                    Debug.WriteLine(ex.ToString());
                }
                else
                {
                    Debug.WriteLine(ex.ToString());
                    throw ex;  // any serious error occurr
                }
            }
        }

        /// <summary>
        /// Sendet die Nachricht an das Connair
        /// </summary>
        /// <param name="socket">Die Socket-Verbindung</param>
        /// <param name="buffer">Der Byte-Buffer</param>
        /// <param name="offset">Das Offset</param>
        /// <param name="size">Die Länge der Nachricht</param>
        /// <param name="timeout">Das Timeout</param>
        //protected static void _send(Socket socket, byte[] buffer, int offset, int size, int timeout)
        //{

        //}
    }
}
