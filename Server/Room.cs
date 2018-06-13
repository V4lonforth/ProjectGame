using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using GameLib.Controllers;
using GameLib.GameObjects;
using GameLib.Info;
using Microsoft.Xna.Framework;
using NetworkLib;
using NetworkLib.Data;

namespace Server
{
    class Room
    {
        private TcpConnector connector;
        private Udp udp;

        //private ProjectilesController projectilesController;

        private StructConverter structConverter;

        private List<Player> players;
        private int lastId;

        private double lastIterationTime;
        private int gameTime;

        private bool roomClosed;

        private ShipInfo[] shipsInfo;
        private BaseProjectilesController projectilesController;

        private const double iterationTime = 1d / 60d;

        public Room(int listeningPort, int sendingPort, ShipInfo[] ships)
        {
            shipsInfo = ships;
            roomClosed = false;
            lastId = 0;
            players = new List<Player>();
            structConverter = new StructConverter();
            //projectilesController = new ProjectilesController();
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, listeningPort);
            udp = new Udp(localEndPoint);
            connector = new TcpConnector(localEndPoint);
        }

        public void StartGame()
        {
            gameTime = 0;
            lastIterationTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
        }

        private bool TryConnect()
        {
            Socket socket = connector.Connect();
            if (socket != null)
            {
                lastId++;
                Tcp tcp = new Tcp(socket);
                BaseShip ship = new BaseShip(shipsInfo[0], null, lastId, Vector2.Zero, lastId);
                Player player = new Player(lastId, tcp, shipsInfo, projectilesController);
                players.Add(player);
                CreateShipActionData data = player.CreateShip();
                structConverter.ConvertStructToBytes(data, DataType.CreateShipAction, out byte[] bytes);
                foreach (Player pl in players)
                    pl.SendTcpData(bytes);
                return true;
            }
            return false;
        }
        
        private void Receive()
        {
            foreach (Player player in players)
            {
                player.ReceiveTcpData();
                player.ReceiveUdpData(udp);
            }
        }

        private void Update()
        {
            double time = DateTime.UtcNow.TimeOfDay.TotalSeconds;
            if (time - lastIterationTime >= iterationTime)
            {
                gameTime++;
                lastIterationTime = time;
                ShipStateData[] shipsStateData = new ShipStateData[players.Count];
                for (int i = 0; i < players.Count; i++)
                    shipsStateData[i] = players[i].GetShipStateData(time - 0.2d);
                structConverter.ConvertStructsToBytes(shipsStateData, DataType.ShipState, out byte[] bytes);
                foreach (Player player in players)
                    udp.SendTo(bytes, player.PlayerEndPoint);
            }
        }
        
        public void Work()
        {
            while (!roomClosed)
            {
                TryConnect();
                Receive();
                Update();
            }
        }
    }
}
