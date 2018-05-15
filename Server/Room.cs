using System;
using System.Collections.Generic;
using System.Net.Sockets;
using NetworkLib;

namespace Server
{
    class Room
    {
        private TcpConnector connector;

        private List<Player> players;
        private int lastId;

        private double lastIterationTime;
        private int gameTime;

        private const double iterationTime = 1d / 60d;

        private const string localAddress = "192.168.43.91";
        private const int listeningPort = 4400;

        private const int sendingPort = 4401;

        public Room()
        {
            lastId = 0;
            players = new List<Player>();

            connector = new TcpConnector(localAddress, listeningPort, sendingPort);
        }

        public void StartGame()
        {
            gameTime = 0;
            lastIterationTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
        }

        public bool TryConnect()
        {
            Socket socket = connector.Connect();
            if (socket != null)
            {
                lastId++;
                Tcp tcp = new Tcp(socket);
                players.Add(new Player(lastId, tcp));
                return true;
            }
            return false;
        }
        
        public void Update()
        {
            if (DateTime.UtcNow.TimeOfDay.TotalSeconds - lastIterationTime >= iterationTime)
            {
                gameTime++;
                lastIterationTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
                Console.WriteLine("Server: game time: {1}, time: {0}", lastIterationTime, gameTime);
                foreach (Player player in players)
                    player.Update();
            }
        }
    }
}
