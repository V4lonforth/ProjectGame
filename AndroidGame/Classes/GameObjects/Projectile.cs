using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Physics;
using AndroidGame.Serialization;
using AndroidGame.GameObjects.Base;
using AndroidGame.GameObjects.Ships;

namespace AndroidGame.GameObjects
{
    class Projectile : PhysicalObject, IPhysicalObject
    {
        public bool IsDestroyed { get; private set; }

        public float Damage { get; private set; }
        public int Team { get; private set; }

        private float timeToDestroy;
        private const float liveTime = 5f;

        public Projectile(ProjectileInfo info, Texture2D[] sprites) 
            : base(new Drawable(sprites[info.spriteIndex], size: info.spriteSize), info.speed, PhysicalObjectType.Projectile, Vector2.Zero, Vector2.Zero, Vector2.Zero, info.bodyInfo)
        {
        }

        public void Launch(Vector2 pos, Vector2 dir, float damage, int team)
        {
            Team = team;
            IsDestroyed = false;
            Position = pos;
            LookingDirection = MovementDirection = dir;
            Damage = damage;
            timeToDestroy = liveTime;
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
            timeToDestroy -= deltaTime;
            if (timeToDestroy <= 0f)
                IsDestroyed = true;
        }

        protected override bool OnCollision(Body body)
        {
            switch(body.Parent.Type)
            {
                case PhysicalObjectType.Ship:
                    if (((Ship)body.Parent).Team == Team)
                        return false;
                    IsDestroyed = true;
                    return true;
            }
            return false;
        }
    }
}
