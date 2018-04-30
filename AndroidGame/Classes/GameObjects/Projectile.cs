using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Physics;
using AndroidGame.Serialization;
using AndroidGame.GameObjects.Base;
using AndroidGame.GameObjects.Ships;
using AndroidGame.Controllers;
using AndroidGame.Geometry;

namespace AndroidGame.GameObjects
{
    public class Projectile : PhysicalObject, IPhysicalObject
    {
        private ParticleSpawner particleSpawner;

        public bool IsDestroyed { get; private set; }

        public float Damage { get; private set; }
        public int Team { get; private set; }

        private float rotationSpeed;

        private float timeToDestroy;
        private const float liveTime = 5f;

        public Projectile(ProjectileInfo info, Texture2D[] sprites, ParticleSystem particleSystem) 
            : base(new Drawable(sprites[info.projectileType], size: info.spriteSize), 0, PhysicalObjectType.Projectile, Vector2.Zero, Vector2.Zero, Vector2.Zero, info.bodyInfo)
        {
            particleSpawner = new ParticleSpawner(particleSystem, this, info.spawnerInfo);
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
            particleSpawner.Update(deltaTime);
            timeToDestroy -= deltaTime;
            LookingDirection = Functions.RotateVector2(LookingDirection, rotationSpeed * deltaTime);
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
