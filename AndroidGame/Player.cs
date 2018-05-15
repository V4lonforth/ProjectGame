using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using NetworkLib;

namespace AndroidGame
{
    class Player
    {
        private Udp udp;
        private Tcp tcp;

        private const string serverAddress = "192.168.43.91";
        private const int listeningPort = 4401;
        private const int sendingPort = 4400;

        public Player()
        {
            tcp = new Tcp(new IPEndPoint(IPAddress.Parse(serverAddress), sendingPort));
            udp = new Udp(tcp.LocalEndPoint);
        }

    }
}