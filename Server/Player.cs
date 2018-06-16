using GameLib.GameObjects;
using NetworkLib;
using NetworkLib.Data;
using System.Net;
using GameLib.Controllers;
using GameLib.Info;
using Microsoft.Xna.Framework;
using System;

namespace Server
{
    public class Player
    {
        private ShipController shipController;
        private BaseProjectilesController projectilesController;
        private StructConverter structConverter;

        private Tcp connection;
        private ShipInfo[] shipsInfo;
        

        public int Id { get; private set; }
        public IPEndPoint PlayerEndPoint { get { return connection.RemoteEndPoint; } }

        public double LastReceivedDataTime
        {
            get
            {
                return shipController.Time;
            }
        }

        private double initialTime;
        private double timeDiff;

        private double timeDiffInterval = 0.08d;
        private const double timeDiffIncreasing = 0.004d;

        private const int historySize = 100;
        private const int maxMissingDataCount = 5;

        private const double timeEpsilon = 0.002d;
        private const float timeToCheck = 0.2f;
        private const float averageDeltaTime = 1 / 60f;

        public Player(int id, Tcp connect, ShipInfo[] info, BaseProjectilesController projController)
        {
            structConverter = new StructConverter();
            shipsInfo = info;
            Id = id;
            connection = connect;
            projectilesController = projController;
            shipController = new ShipController(historySize);

            Initialize();
        }
        private void Initialize()
        {
            structConverter.ConvertStructToBytes(new InitializeData() { id = Id }, DataType.Initialize, out byte[] bytes);
            connection.Send(bytes);
            initialTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
        }
        public void SendTcpData(byte[] bytes)
        {
            connection.Send(bytes);
        }
        public CreateShipActionData GetCreateShipActionData()
        {
            CreateShipActionData data = shipController.GetCreateShipActionData();
            data.owner = ShipOwner.Player;
            return data;
        }
        public void CreateShip()
        {
            shipController.SetShip(new BaseShip(shipsInfo[0], projectilesController, Id, Vector2.Zero, Id, true), new BaseShip(shipsInfo[0], null, Id, Vector2.Zero, Id, false));
        }
        public ShipStateData GetShipStateData()
        {
            return shipController.GetShipState(shipController.FirstElementNumber);
        }
        public ShipStateData GetShipStateData(double time)
        {
            double playerTime = time - timeDiff;
            if (shipController.Time < playerTime)
                timeDiff += timeDiffIncreasing;
            else if (shipController.Time > playerTime + timeDiffInterval)
                timeDiff -= timeDiffIncreasing;
            return shipController.GetShipState(time - timeDiff);
        }
        public void ReceiveUdpData(Udp udp)
        {
            EndPoint endPoint = PlayerEndPoint;
            udp.ReceiveFrom(out byte[] bytes, ref endPoint);

            while (bytes != null)
            {
                int index = 0;
                while (index < bytes.Length)
                {
                    if (structConverter.GetDataType(bytes, 0) != DataType.Time)
                        break;
                    index += 1 + structConverter.ConvertBytesToStruct(bytes, 1, out TimeData timeData);

                    DataType dataType = structConverter.GetDataType(bytes, index);
                    index++;
                    switch (dataType)
                    {
                        case DataType.Input:
                            index += structConverter.ConvertBytesToStruct(bytes, index, out InputData inputData);
                            shipController.AddInputData(ref timeData, ref inputData);
                            break;
                        default:
                            break;
                    }
                }
                udp.ReceiveFrom(out bytes, ref endPoint);
            }
        }
        public void ReceiveTcpData()
        {
            connection.Receive(out byte[] bytes);
            if (bytes != null)
            {
                int index = 0;
                while (index < bytes.Length)
                {
                    DataType dataType = structConverter.GetDataType(bytes, index);
                    index++;
                    switch  (dataType)
                    {
                        case DataType.InitialTime:
                            index += structConverter.ConvertBytesToStruct(bytes, index, out double time);
                            timeDiff = (DateTime.UtcNow.TimeOfDay.TotalSeconds - initialTime) / 2 + initialTime - time;
                            break;
                    }
                }
            }
        }
    }
}