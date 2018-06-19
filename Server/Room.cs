﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using GameLib.Controllers;
using GameLib.GameObjects;
using GameLib.Info;
using GameLib.Physics;
using Microsoft.Xna.Framework;
using NetworkLib;
using NetworkLib.Data;

namespace Server
{
    class Room
    {
        private TcpConnector connector;
        private Udp udp;

        private StructConverter structConverter;

        private List<Player> players;
        private List<ShipController> bots;
        private int lastId;

        private double lastIterationTime;
        private int gameTime;

        private bool roomClosed;

        private ShipInfo[] shipsInfo;
        private BaseProjectilesController projectilesController;
        private PhysicsController physicsController;

        private const double disconnectTime = 3d;
        private const double iterationTime = 1d / 60d;

        public Room(int listeningPort, int sendingPort, ShipInfo[] ships)
        {
            physicsController = new PhysicsController();
            Body.SetPhysicsController(physicsController);
            shipsInfo = ships;
            roomClosed = false;
            lastId = 0;
            bots = new List<ShipController>();
            players = new List<Player>();
            structConverter = new StructConverter();
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, listeningPort);
            udp = new Udp(localEndPoint);
            connector = new TcpConnector(localEndPoint);
            projectilesController = new BaseProjectilesController(100);
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
                Player player = new Player(lastId, tcp, shipsInfo, projectilesController);
                player.CreateShip();
                CreateShipActionData[] data = new CreateShipActionData[bots.Count + players.Count + 1];
                data[0] = player.GetCreateShipActionData();
                structConverter.ConvertStructToBytes(data[0], DataType.CreateShipAction, out byte[] bytes);
                foreach (Player pl in players)
                    pl.SendTcpData(bytes);
                for (int i = 0; i < players.Count; i++)
                    data[i + 1] = players[i].GetCreateShipActionData();
                players.Add(player);
                for (int i = 0; i < bots.Count; i++)
                    data[i + players.Count] = bots[i].GetCreateShipActionData();
                structConverter.ConvertStructsToBytes(data, DataType.CreateShipAction, out bytes);
                player.SendTcpData(bytes);
                return true;
            }
            return false;
        }

        private void Receive()
        {
            foreach (Player player in players)
                player.ReceiveTcpData();
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            bool finished = false;
            do
            {
                udp.ReceiveFrom(out byte[] bytes, ref endPoint);
                if (bytes == null)
                    finished = true;
                else
                {
                    foreach (Player player in players)
                        if (player.PlayerEndPoint.Equals(endPoint))
                        {
                            player.ReceiveUdpData(bytes);
                            break;
                        }
                }
            } while (!finished);
        }
        private bool CheckPlayerConnection(Player player)
        {
            double currentTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
            if (currentTime - player.LastCheckedTime > disconnectTime)
                return false;
            player.CheckConnection();
            return true;
        }
        private void Update()
        {
            double time = DateTime.UtcNow.TimeOfDay.TotalSeconds;
            byte[] bytes;
            if (time - lastIterationTime >= iterationTime)
            {
                gameTime++;
                float deltaTime = (float)(time - lastIterationTime);
                lastIterationTime = time;
                for (int i = 0; i < players.Count; i++)
                    if (players[i].Initialized && !CheckPlayerConnection(players[i]))
                    {
                        structConverter.ConvertStructToBytes(new DestroyShipActionData() { id = players[i].Id }, DataType.DestroyShipAction, out bytes);
                        players.RemoveAt(i);
                        i--;
                        foreach (Player player in players)
                            player.SendTcpData(bytes);
                    }
                ShipStateData[] shipsStateData = new ShipStateData[players.Count];
                projectilesController.Update(deltaTime);
                physicsController.CheckCollisions();

                for (int i = 0; i < players.Count; i++)
                {
                    shipsStateData[i] = players[i].GetShipStateData(time - 0.2d);
                    if (shipsStateData[i].shipData.health <= 0f)
                    {
                        DestroyShipActionData destroyShipActionData = new DestroyShipActionData()
                        {
                            id = shipsStateData[i].shipId
                        };
                        structConverter.ConvertStructToBytes(destroyShipActionData, DataType.DestroyShipAction, out bytes);
                        foreach (Player player in players)
                            player.SendTcpData(bytes);
                        players[i].CreateShip();
                        structConverter.ConvertStructToBytes(players[i].GetCreateShipActionData(), DataType.CreateShipAction, out bytes);
                        foreach (Player player in players)
                            player.SendTcpData(bytes);
                    }
                }
                structConverter.ConvertStructsToBytes(shipsStateData, DataType.ShipState, out bytes);
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
