using System;
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
        private ShipController shipController;

        private StructConverter converter;

        private const string serverAddress = "192.168.42.96";
        private const int listeningPort = 4401;
        private const int sendingPort = 4400;

        public NetController(ShipsController shipsController)
        {
            converter = new StructConverter();
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), sendingPort);
            tcp = new Tcp(serverEndPoint);
            udp = new ConnectedUdp(tcp.LocalEndPoint, serverEndPoint);
            this.shipsController = shipsController;
            shipController = new ShipController();
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
                            if (createShipActionData.id == id)
                                shipController = shipsController.CreatePlayerShip(sender, createShipActionData.position, createShipActionData.id);
                            break;
                    }
                }
            }
        }
        public void Update()
        {
            ReceiveTCPData();
            udp.Receive(out byte[] bytes);
            if (bytes != null)
            {
                int index = 0;
                DataType type = converter.GetDataType(bytes, index);
                index++;
                switch (type)
                {
                    case DataType.Ship:
                        converter.ConvertBytesToStruct(bytes, index, out ShipData shipData);
                        break;
                    case DataType.Input:
                        converter.ConvertBytesToStruct(bytes, index, out InputData inputData);
                        break;
                    case DataType.ShipState:
                        converter.ConvertBytesToStruct(bytes, index, out ShipStateData shipStateData);
                        ShipStateData data = shipController.GetShipState(shipController.FirstElementNumber);
                        System.Diagnostics.Debug.WriteLine($"X:{shipStateData.shipData.position.X}, Y:{shipStateData.shipData.position.Y}; X:{data.shipData.position.X}, Y:{data.shipData.position.Y}");
                        //shipController.AddInputData(ref shipStateData.timeData, ref shipStateData.inputData);
                        break;
                }
            }
        }
    }
}