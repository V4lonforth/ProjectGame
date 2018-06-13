using Microsoft.Xna.Framework;

namespace NetworkLib.Data
{
    public struct ShipData
    {
        public Vector2 position;
        public Vector2 movementDirection;
        public Vector2 lookingDirection;
        public float speed;

        public float timeToRotate;
        public float timeToShoot;
    }
}