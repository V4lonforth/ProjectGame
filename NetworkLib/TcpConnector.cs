using System.Net;
using System.Net.Sockets;

namespace NetworkLib
{
    public class TcpConnector
    {
        private TcpListener listener;

        public TcpConnector(IPEndPoint localEndPoint)
        {
            listener = new TcpListener(localEndPoint);
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
