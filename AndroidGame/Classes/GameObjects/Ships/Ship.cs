using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Serialization;
using AndroidGame.Controllers;
using AndroidGame.Physics;
using AndroidGame.Geometry;
using AndroidGame.GameObjects.Base;

namespace AndroidGame.GameObjects.Ships
{
    public class Ship : BaseShip, IPhysicalObject
    {
        private ParticleSpawner particleSpawner;

        private Gun gun;

        private float timeToRotateLeft;
        private const float timeToRotate = 0.6f;

        public Ship(ShipInfo shipInfo, Texture2D[] shipSprites, ProjectilesController projController, ParticleSystem parSystem, int team, Vector2 pos) 
            : base(shipInfo, new Drawable(shipSprites[shipInfo.shipType], size: shipInfo.spriteSize, col: shipInfo.color), team, pos)
        {
            gun = new Gun(this, shipInfo.gunInfo, projController, team);
            particleSpawner = new ParticleSpawner(parSystem, this, shipInfo.spawnerInfo);
            rotationSpeed = shipInfo.rotationSpeed;
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
            particleSpawner.Update(deltaTime);
            gun.Update(deltaTime);
            if (!gun.IsShooting)
            {
                timeToRotateLeft -= deltaTime;
                if (timeToRotateLeft <= 0f && isAccelerating)
                    LookingDirection = Functions.CircleLerp(LookingDirection, MovementDirection, rotationSpeed * deltaTime);
            }
        }
        
    }
}