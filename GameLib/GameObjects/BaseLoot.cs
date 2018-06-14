using Microsoft.Xna.Framework;
using GameLib.Physics;
using GameLib.GameObjects.Base;
using GameLib.Info;

namespace GameLib.GameObjects
{
    public class BaseLoot : PhysicalObjectAcc, ILoot
    {
        private float experience;

        private Vector2 startDirection;

        public bool IsActive
        {
            get
            {
                return body.IsActive;
            }
            private set
            {
                body.IsActive = value;
            }
        }

        private new const float maxSpeed = 3000f;
        private new const float acceleration = 800f;

        private const float lootDistance = 30f;
        private static Vector2 size = new Vector2(50f);
        private static Color color = Color.White;

        public BaseLoot(BodyInfo bodyInfo)
            : base(maxSpeed, acceleration, PhysicalObjectType.Loot, Vector2.Zero, Vector2.Zero, Vector2.Zero, bodyInfo, false)
        {
        }
        
        protected override bool OnCollision(Body body)
        {
            PhysicalObject parent = (PhysicalObject)body.Parent;
            switch (parent.Type)
            {
                case PhysicalObjectType.Ship:
                    if (body.GetType() == typeof(BaseShip))
                    {
                        isAccelerating = true;
                        MovementDirection = parent.Position - Position;
                        float distance = MovementDirection.Length();
                        if (distance <= lootDistance)
                        {
                            ((BaseShip)body.Parent).GainExperience(experience);
                            IsActive = false;
                            return true;
                        }
                        MovementDirection /= distance;
                    }
                    break;
            }
            return false;
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public void Launch(float exp, Vector2 pos, Vector2 dir)
        {
            LookingDirection = startDirection = dir;
            experience = exp;
            Position = pos;
            IsActive = true;
        }
    }
}