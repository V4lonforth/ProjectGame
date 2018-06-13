using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using GameLib.Info;
using Microsoft.Xna.Framework;

namespace Server
{
    class Server
    {
        private const int listeningPort = 4400;
        private const int sendingPort = 4401;

        static void Main(string[] args)
        {
            Room room = new Room(listeningPort, sendingPort, ShipInfo.GetShipsInfo());
            Thread thread = new Thread(room.Work);
            thread.Start();
        }

    }
}
