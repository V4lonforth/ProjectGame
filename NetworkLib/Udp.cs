using System;
using System.Net;
using System.Net.Sockets;

namespace NetworkLib
{
    public class Udp
    {
        private Socket socket;
        
        private const int maxBufferSize = 1024;

        public Udp(EndPoint localEP)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(localEP);
        }

        public Udp(Socket socket)
        {
            this.socket = socket;
        }

        public Exception SendTo(byte[] bytes, EndPoint remotePoint)
        {
            try
            {
                socket.SendTo(bytes, remotePoint);
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }

        public Exception ReceiveFrom(out byte[] bytes, ref EndPoint remotePoint)
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[maxBufferSize];
                int size = 0;
                try
                {
                    size = socket.ReceiveFrom(buffer, ref remotePoint);
                }
                catch (Exception e)
                {
                    bytes = null;
                    return e;
                }
                bytes = new byte[size];
                Array.Copy(buffer, bytes, size);
                return null;
            }
            bytes = null;
            return null;
        }
    }
}
