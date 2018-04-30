using Microsoft.Xna.Framework;
using AndroidGame.Serialization;
using AndroidGame.Controllers;
using AndroidGame.Geometry;
using AndroidGame.GameObjects.Base;

namespace AndroidGame.GameObjects.Ships
{
    public class Gun
    {
        private PhysicalObject parent;

        private Vector2[] shootStartPositions;
        private int positionIndex;

        private float shootTime;
        private float timeToShoot;

        private float projectileSpeed;
        private float damage;

        private int projectilesType;
        private int team;

        public bool IsShooting { get; set; }

        private ProjectilesController projectilesController;

        public Gun(PhysicalObject par, GunInfo gunInfo, ProjectilesController pController, int team)
        {
            parent = par;
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
                    projectilesController.LaunchProjectile(Functions.RotateVector2(shootStartPositions[positionIndex], parent.LookingDirection) + parent.Position, parent.LookingDirection, parent.LookingDirection * projectileSpeed + parent.MovementDirection * parent.Speed, damage, team, projectilesType);
                    positionIndex = (positionIndex + 1) % shootStartPositions.Length;
                    timeToShoot = shootTime;
                }
            }
            else
                timeToShoot -= deltaTime;
        }
    }
}