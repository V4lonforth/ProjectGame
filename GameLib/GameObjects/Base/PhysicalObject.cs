using Microsoft.Xna.Framework;
using GameLib.Physics;
using GameLib.Info;

namespace GameLib.GameObjects.Base
{
    public abstract class PhysicalObject : IPhysicalObject
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

        public float Speed { get; protected set; }
        
        public PhysicalObjectType Type { get; private set; }
        
        public PhysicalObject(float sp, PhysicalObjectType t, Vector2 pos, Vector2 movDir, Vector2 lookDir, BodyInfo bodyInfo, bool isActive = true)
        {
            body = new Body(bodyInfo, OnCollision, pos, lookDir, this, isActive);
            Position = pos;
            MovementDirection = movDir;
            LookingDirection = lookDir;
            Speed = sp;
            Type = t;
        }
        
        public void Update(float deltaTime)
        {
            Position += Speed * deltaTime * MovementDirection;
        }

        protected abstract bool OnCollision(Body body);
    }
}