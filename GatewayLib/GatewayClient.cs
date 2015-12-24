using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GatewayLib
{
    public static class GatewayClient
    {
        public static void Send(String text, string networkIp, int port)
        {
            String server = networkIp;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress address = IPAddress.Parse(server);
            IPEndPoint remoteEP = new IPEndPoint(address, port);
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
                    Debug.WriteLine("Send Completed, error : " + e.SocketError + "Bytes transferred: " +
                                    e.BytesTransferred);
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
                    throw ex; // any serious error occurr
                }
            }
        }
    }
}
