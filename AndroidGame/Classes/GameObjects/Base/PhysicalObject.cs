using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Physics;
using AndroidGame.Serialization;

namespace AndroidGame.GameObjects.Base
{
    public abstract class PhysicalObject : IPhysicalObject
    {
        protected Body body;
        private Drawable drawable;

        public Vector2 Position
        {
            get
            {
                return body.Position;
            }
            protected set
            {
                drawable.Position = value;
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
                drawable.Rotation = (float)Math.Atan2(value.Y, value.X);
                body.Direction = value;
            }
        }

        protected float speed;
        
        public PhysicalObjectType Type { get; private set; }
        
        public PhysicalObject(Drawable draw, float sp, PhysicalObjectType t, Vector2 pos, Vector2 movDir, Vector2 lookDir, BodyInfo bodyInfo)
        {
            body = new Body(bodyInfo, OnCollision, pos, lookDir, this);
            drawable = draw;
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
        public void Draw(SpriteBatch spriteBatch)
        {
            drawable.Draw(spriteBatch);
        }

        protected abstract bool OnCollision(Body body);
    }
}