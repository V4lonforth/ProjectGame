using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Serialization;
using AndroidGame.Controllers;
using AndroidGame.Physics;
using AndroidGame.Geometry;
using AndroidGame.GameObjects.Base;

namespace AndroidGame.GameObjects.Ships
{
    class Ship : PhysicalObjectAcc, IPhysicalObject
    {
        private Gun gun;

        private float rotationSpeed;
        private float timeToRotateLeft;
        
        private float health;

        public int Team { get; private set; }
        public bool IsDestroyed { get; private set; }

        public float DroppingExperience { get; protected set; }

        private const float timeToRotate = 0.6f;

        public Ship(ShipInfo shipInfo, Texture2D[] shipSprites, ProjectilesController pController, int team, Vector2 pos, Vector2 dir) 
            : base(new Drawable(shipSprites[shipInfo.shipType]), shipInfo.maxSpeed, shipInfo.acceleration, PhysicalObjectType.Ship, pos, dir, dir, shipInfo.bodyInfo)
        {
            gun = new Gun(shipInfo.gunInfo, pController, team);
            IsDestroyed = false;
            Team = team;
            health = shipInfo.health;
            rotationSpeed = shipInfo.rotationSpeed;
        }

        private void Destroy()
        {
            IsDestroyed = true;
        }

        public void SetMovementDirection(Vector2 dir)
        {
            if (dir == Vector2.Zero)
                isAccelerating = false;
            else
            {
                isAccelerating = true;
                MovementDirection = dir;
            }
        }
        public void SetAttackDirection(Vector2 dir)
        {
            if (dir == Vector2.Zero)
            {
                timeToRotateLeft = timeToRotate;
                gun.IsShooting = false;
            }
            else
            {
                LookingDirection = dir;
                gun.IsShooting = true;
            }
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
            gun.Update(Position, LookingDirection, deltaTime);
            if (!gun.IsShooting)
            {
                timeToRotateLeft -= deltaTime;
                if (timeToRotateLeft <= 0f && isAccelerating)
                    LookingDirection = Functions.CircleLerp(LookingDirection, MovementDirection, rotationSpeed);
            }
        }
        
        protected override bool OnCollision(Body body)
        {
            switch (body.Parent.Type)
            {
                case PhysicalObjectType.Projectile:
                    if (((Projectile)body.Parent).Team != Team)
                    {
                        health -= ((Projectile)body.Parent).Damage;
                        if (health <= 0f)
                        {
                            Destroy();
                            return true;
                        }
                    }
                    return false;
            }
            return false;
        }
    }
}