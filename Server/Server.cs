using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            Room room = new Room();

            Thread.Sleep(2000);

            while(true)
            {
                if (room.TryConnect())
                    room.StartGame();
                room.Update();
            }
        }
    }
}
