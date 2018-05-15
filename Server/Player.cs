using System;
using Microsoft.Xna.Framework;
using AndroidGame.Geometry;
using AndroidGame.GameObjects.Ships;
using AndroidGame.Serialization;
using NetworkLib;

namespace Server
{
    public class Player
    {
        private ServerShip ship;

        private Tcp connection;

        public ShipData[] shipDataHistory;
        private int currentDataIndex;

        public int Id { get; private set; }
        public double LastReceivedDataTime
        {
            get
            {
                return shipDataHistory[currentDataIndex].time;
            }
        }

        private const int historySize = 100;

        private const double timeEpsilon = 0.002d;

        public Player (int id, Tcp connect)
        {
            Id = id;
            connection = connect;
            shipDataHistory = new ShipData[historySize];
            currentDataIndex = 0;
        }

        public void Update()
        {
        }

        public void CreateShip(ShipInfo shipInfo, int team, Vector2 position)
        {
            ship = new ServerShip(shipInfo, team, position);
        }
        public BaseShip GetShipState(double time)
        {
            int index = currentDataIndex;
            while (shipDataHistory[index].time > time)
            {
                index = GetPreviousIndex(index);
                if (index == currentDataIndex)
                {
                    throw new Exception();
                }
            }

            ship.SetState(Interpolate(ref shipDataHistory[index], ref shipDataHistory[GetNextIndex(index)], time));
            return ship;
        }

        private ShipData Interpolate(ref ShipData first, ref ShipData second, double time)
        {
            if (time - first.time <= timeEpsilon)
                return first;
            if (second.time - time <= timeEpsilon)
                return second;

            float koeff = (float)((time - first.time) / (second.time - first.time));
            ShipData interpolated = new ShipData()
            {
                speed = Functions.Interpolate(first.speed, second.speed, koeff),
                position = Functions.Interpolate(first.position, second.position, koeff),
                time = time
            };
            interpolated.lookingDirection = Functions.CircleLerp(first.lookingDirection, second.lookingDirection, ship.Speed);
            interpolated.movementDirection = Functions.CircleInterpolate(first.movementDirection, second.movementDirection, koeff);

            return interpolated;
        }

        private int GetNextIndex(int index)
        {
            return (index + 1) % historySize;
        }
        private int GetPreviousIndex(int index)
        {
            return (index + historySize - 1) % historySize;
        }
    }
}