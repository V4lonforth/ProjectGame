using Microsoft.Xna.Framework;

namespace NetworkLib
{
    public struct ShipData
    {
        public Vector2 position;
        public Vector2 lookingDirection;
        public Vector2 movementDirection;
        public float speed;

        public int gameTime;
        public double time;
    }
}