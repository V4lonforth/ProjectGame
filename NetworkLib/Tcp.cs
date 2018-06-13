using System;
using System.Net;
using System.Net.Sockets;

namespace NetworkLib
{
    public class Tcp
    {
        private Socket socket;

        private const int maxBufferSize = 1024;

        public IPEndPoint LocalEndPoint
        {
            get
            {
                return (IPEndPoint)socket.LocalEndPoint;
            }
        }
        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return (IPEndPoint)socket.RemoteEndPoint;
            }
        }

        public Tcp(Socket tcpSocket)
        {
            socket = tcpSocket;
        }

        public Tcp(EndPoint endPoint)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);
        }

        public Exception Send(byte[] bytes)
        {
            try
            {
                socket.Send(bytes);
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }

        public Exception Receive(out byte[] bytes)
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[maxBufferSize];
                int size = 0;
                try
                {
                    size = socket.Receive(buffer);
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
