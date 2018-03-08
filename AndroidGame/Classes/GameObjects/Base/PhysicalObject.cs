using Microsoft.Xna.Framework;
using AndroidGame.Physics;

namespace AndroidGame.GameObjects.Base
{
    public class PhysicalObject
    {
        protected Body body;

        public Vector2 Position
        {
            get
            {
                return body.Position;
            }
            protected set
            {
                body.Position = value;
            }
        }
        public Vector2 MovementDirection { get; protected set; }
        public Vector2 LookingDirection
        {
            get
            {
                return body.Direction;
            }
            protected set
            {
                body.Direction = value;
            }
        }

        protected float speed;
        
        public PhysicalObjectType Type { get; private set; }
        
        public PhysicalObject(float sp, PhysicalObjectType t, Vector2 pos, Vector2 movDir, Vector2 lookDir, Body baseBody)
        {
            body = new Body(baseBody, pos, lookDir, this, true);
            Position = pos;
            MovementDirection = movDir;
            LookingDirection = lookDir;
            speed = sp;
            Type = t;
        }
        
        public void Update(float deltaTime)
        {
            Position += speed * deltaTime * MovementDirection;
        }
    }
}