using Microsoft.Xna.Framework;
using GameLib.Info;
using GameLib.Controllers;
using GameLib.Geometry;
using GameLib.GameObjects.Base;

namespace GameLib.GameObjects
{
    public class Gun
    {
        private PhysicalObject parent;

        private Vector2[] shootStartPositions;
        private int positionIndex;

        private float shootTime;
        public float timeToShoot;

        private float projectileSpeed;
        private float damage;

        private int projectilesType;
        private int team;

        private bool isActive;
        public bool IsShooting { get; set; }

        private BaseProjectilesController projectilesController;

        public Gun(PhysicalObject par, GunInfo gunInfo, BaseProjectilesController pController, int team, bool active)
        {
            parent = par;
            isActive = active;
            projectileSpeed = gunInfo.projectileSpeed;
            shootTime = 1f / gunInfo.shootRate;
            projectilesType = gunInfo.projectilesType;
            shootStartPositions = gunInfo.shootPositions;
            damage = gunInfo.damage;
            timeToShoot = 0f;
            positionIndex = 0;
            IsShooting = false;
            projectilesController = pController;
            this.team = team;
        }
        
        public void Update(float deltaTime)
        {
            if (timeToShoot <= 0f)
            {
                if (IsShooting)
                {
                    if (isActive)
                    {
                        projectilesController.LaunchProjectile(Functions.RotateVector2(shootStartPositions[positionIndex], parent.LookingDirection) + parent.Position,
                            parent.LookingDirection, parent.LookingDirection * projectileSpeed + parent.MovementDirection * parent.Speed, damage, team, projectilesType);
                        positionIndex = (positionIndex + 1) % shootStartPositions.Length;
                    }
                    timeToShoot = shootTime;
                }
            }
            else
                timeToShoot -= deltaTime;
        }
    }
}