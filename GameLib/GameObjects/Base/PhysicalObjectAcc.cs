using Microsoft.Xna.Framework;
using GameLib.Info;

namespace GameLib.GameObjects.Base
{
    public abstract class PhysicalObjectAcc : PhysicalObject, IPhysicalObject
    {
        protected float acceleration;
        protected float maxSpeed;

        protected bool isAccelerating;
        
        public PhysicalObjectAcc(float maxSp, float acc, PhysicalObjectType t, Vector2 pos, Vector2 movDir, Vector2 lookDir, BodyInfo bodyInfo, bool isActive = true) 
            : base(0f, t, pos, movDir, lookDir, bodyInfo, isActive)
        {
            maxSpeed = maxSp;
            acceleration = acc;
            isAccelerating = false;
        }
        
        public new void Update(float deltaTime)
        {
            if (isAccelerating)
            {
                if (Speed < maxSpeed)
                    Speed += acceleration * deltaTime;
                else
                    Speed = maxSpeed;
            }
            else
            {
                if (Speed > 0f)
                    Speed -= acceleration * deltaTime;
                else
                    Speed = 0f;
            }
            base.Update(deltaTime);
        }
    }
}