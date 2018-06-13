using System;
using System.Net;
using System.Net.Sockets;

namespace NetworkLib
{
    public class ConnectedUdp : Udp
    {
        private EndPoint remotePoint;

        public ConnectedUdp(EndPoint localEP, EndPoint endPoint) : base(localEP)
        {
            remotePoint = endPoint;
        }

        public ConnectedUdp(Socket socket, EndPoint endPoint) : base(socket)
        {
            remotePoint = endPoint;
        }

        public Exception Send(byte[] bytes)
        {
            return SendTo(bytes, remotePoint);
        }
        public Exception Receive(out byte[] bytes)
        {
            EndPoint endPoint = remotePoint;
            Exception e = ReceiveFrom(out bytes, ref endPoint);
            if (((IPEndPoint)endPoint).Address.Equals(((IPEndPoint)remotePoint).Address))
                return e;
            else
                return new Exception("Data received from wrong address");
        }
    }
}
