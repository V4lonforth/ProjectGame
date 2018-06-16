using Microsoft.Xna.Framework;

namespace NetworkLib.Data
{
    public struct CreateShipActionData
    {
        public ShipOwner owner;
        public int id;
        public int team;
        public int shipType;
        public Vector2 position;
    }
}
