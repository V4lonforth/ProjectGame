using System;
using AndroidGame.GameObjects.Base;
using AndroidGame.GameObjects.Ships;
using AndroidGame.Serialization;
using Microsoft.Xna.Framework;
using NetworkLib;

namespace Server
{
    class ServerShip : BaseShip
    {
        public ServerShip(ShipInfo shipInfo, int team, Vector2 pos) : base(shipInfo, null, team, pos)
        {
        }

        public void SetState(ShipData data)
        {
            Position = data.position;
            LookingDirection = data.lookingDirection;
            MovementDirection = data.movementDirection;
            Speed = data.speed;
        }
    }
}
