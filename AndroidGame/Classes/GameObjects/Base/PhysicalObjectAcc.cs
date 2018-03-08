using Microsoft.Xna.Framework;
using AndroidGame.Physics;

namespace AndroidGame.GameObjects.Base
{
    class PhysicalObjectAcc : PhysicalObject
    {
        protected float acceleration;
        protected float maxSpeed;

        protected bool isAccelerating;
        
        public PhysicalObjectAcc(float maxSp, float acc, PhysicalObjectType t, Vector2 pos, Vector2 movDir, Vector2 lookDir, Body baseBody) : base(0f, t, pos, movDir, lookDir, baseBody)
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