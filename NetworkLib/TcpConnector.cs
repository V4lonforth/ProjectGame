using System.Net;
using System.Net.Sockets;

namespace NetworkLib
{
    public class TcpConnector
    {
        private TcpListener listener;

        private int listeningPort;
        private int sendingPort;

        public TcpConnector(string localAddress, int listenPort, int sendPort)
        {
            listeningPort = listenPort;
            sendingPort = sendPort;

            listener = new TcpListener(IPAddress.Parse(localAddress), listeningPort - 2);
            listener.Start();
        }

        public Socket Connect()
        {
            if (listener.Pending())
                return listener.AcceptSocket();
            return null;
        }

        public void Stop()
        {
            listener.Stop();
        }
    }
}
