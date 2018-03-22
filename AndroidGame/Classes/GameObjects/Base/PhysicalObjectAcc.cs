using Microsoft.Xna.Framework;
using AndroidGame.Serialization;

namespace AndroidGame.GameObjects.Base
{
    public abstract class PhysicalObjectAcc : PhysicalObject, IPhysicalObject
    {
        protected float acceleration;
        protected float maxSpeed;

        protected bool isAccelerating;
        
        public PhysicalObjectAcc(Drawable draw, float maxSp, float acc, PhysicalObjectType t, Vector2 pos, Vector2 movDir, Vector2 lookDir, BodyInfo bodyInfo) 
            : base(draw, 0f, t, pos, movDir, lookDir, bodyInfo)
        {
            maxSpeed = maxSp;
            acceleration = acc;
            isAccelerating = false;
        }
        
        public new void Update(float deltaTime)
        {
            if (isAccelerating)
            {
                if (speed < maxSpeed)
                    speed += acceleration * deltaTime;
                else
                    speed = maxSpeed;
            }
            else
            {
                if (speed > 0f)
                    speed -= acceleration * deltaTime;
                else
                    speed = 0f;
            }
            base.Update(deltaTime);
        }
    }
}