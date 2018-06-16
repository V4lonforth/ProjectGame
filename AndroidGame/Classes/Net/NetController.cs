using System;
using System.Collections.Generic;
using System.Net;
using AndroidGame.Controllers;
using GameLib.Controllers;
using NetworkLib;
using NetworkLib.Data;

namespace AndroidGame.Net
{
    class NetController
    {
        private int id;

        private ConnectedUdp udp;
        private Tcp tcp;
        private Sender sender;

        private ShipsController shipsController;
        private List<ShipController> shipControllers;

        private StructConverter converter;

        private const string serverAddress = "192.168.43.91";
        private const int listeningPort = 4401;
        private const int sendingPort = 4400;

        public NetController(ShipsController shipsController)
        {
            converter = new StructConverter();
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), sendingPort);
            tcp = new Tcp(serverEndPoint);
            udp = new ConnectedUdp(tcp.LocalEndPoint, serverEndPoint);
            this.shipsController = shipsController;
            shipControllers = new List<ShipController>();
            sender = new Sender(udp);
        }
        private void ReceiveTCPData()
        {
            tcp.Receive(out byte[] bytes);
            if (bytes != null)
            {
                int index = 0;
                while (index < bytes.Length)
                {
                    DataType type = converter.GetDataType(bytes, index);
                    index++;
                    switch (type)
                    {
                        case DataType.Initialize:
                            converter.ConvertStructToBytes(DateTime.UtcNow.TimeOfDay.TotalSeconds, DataType.InitialTime, out byte[] buf);
                            tcp.Send(buf);
                            index += converter.ConvertBytesToStruct(bytes, index, out InitializeData initializeData);
                            id = initializeData.id;
                            break;
                        case DataType.CreateShipAction:
                            index += converter.ConvertBytesToStruct(bytes, index, out CreateShipActionData createShipActionData);
                            switch (createShipActionData.owner)
                            {
                                case ShipOwner.Player:
                                    if (id == createShipActionData.id)
                                        shipControllers.Add(shipsController.CreatePlayerShip(sender, createShipActionData.position, createShipActionData.id, createShipActionData.shipType, createShipActionData.team));
                                    else
                                        shipControllers.Add(shipsController.CreateEnemyPlayerShip(createShipActionData.position, createShipActionData.id, createShipActionData.shipType, createShipActionData.team));
                                    break;
                                case ShipOwner.AI:
                                    shipControllers.Add(shipsController.CreateAIShip(createShipActionData.position, createShipActionData.id, createShipActionData.shipType, createShipActionData.team));
                                    break;
                            }
                            break;
                        case DataType.DestroyShipAction:
                            index += converter.ConvertBytesToStruct(bytes, index, out DestroyShipActionData destroyShipActionData);
                            for (int i = 0; i < shipControllers.Count; i++)
                                if (destroyShipActionData.id == shipControllers[i].Id)
                                    shipControllers.RemoveAt(i);
                            break;
                    }
                }
            }
        }
        private void ReceiveUdpData()
        {
            udp.Receive(out byte[] bytes);
            if (bytes != null)
            {
                int index = 0;
                DataType type = converter.GetDataType(bytes, index);
                index++;
                switch (type)
                {
                    case DataType.Ship:
                        index += converter.ConvertBytesToStruct(bytes, index, out ShipData shipData);
                        break;
                    case DataType.Input:
                        index += converter.ConvertBytesToStruct(bytes, index, out InputData inputData);
                        break;
                    case DataType.ShipState:
                        index += converter.ConvertBytesToStruct(bytes, index, out ShipStateData shipStateData);
                        foreach (ShipController shipController in shipControllers)
                            if (shipController.Id == shipStateData.shipId)
                                shipController.CheckShipData(ref shipStateData);
                        break;
                }
            }
        }
        public void Update()
        {
            ReceiveTCPData();
            ReceiveUdpData();
        }
    }
}