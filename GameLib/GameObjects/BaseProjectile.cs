using Microsoft.Xna.Framework;
using GameLib.Physics;
using GameLib.Info;
using GameLib.Geometry;
using GameLib.GameObjects.Base;

namespace GameLib.GameObjects
{
    public class BaseProjectile : PhysicalObject, IProjectile
    {
        public bool IsDestroyed { get; private set; }

        public float Damage { get; private set; }
        public int Team { get; private set; }

        private float rotationSpeed;

        private float timeToDestroy;
        private const float liveTime = 5f;

        public BaseProjectile (ProjectileInfo info)
            : base(0, PhysicalObjectType.Projectile, Vector2.Zero, Vector2.Zero, Vector2.Zero, info.bodyInfo)
        {
            rotationSpeed = info.rotationSpeed;
        }

        public void Launch(Vector2 pos, Vector2 dir, Vector2 velocity, float damage, int team)
        {
            Team = team;
            IsDestroyed = false;
            Position = pos;
            LookingDirection = dir;
            Speed = velocity.Length();
            MovementDirection = velocity / Speed;
            Damage = damage;
            timeToDestroy = liveTime;
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
            timeToDestroy -= deltaTime;
            LookingDirection = Functions.RotateVector2(LookingDirection, rotationSpeed * deltaTime);
            if (timeToDestroy <= 0f)
                IsDestroyed = true;
        }

        protected override bool OnCollision(Body body)
        {
            if (!IsDestroyed)
            {
                PhysicalObject parent = (PhysicalObject)body.Parent;
                switch (parent.Type)
                {
                    case PhysicalObjectType.Ship:
                        if (((BaseShip)body.Parent).Team == Team)
                            return false;
                        IsDestroyed = true;
                        return true;
                }
            }
            return false;
        }
    }
}
