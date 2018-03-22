using Microsoft.Xna.Framework;
using AndroidGame.Serialization;
using AndroidGame.Controllers;
using AndroidGame.Geometry;

namespace AndroidGame.GameObjects.Ships
{
    class Gun
    {
        private Vector2[] shootStartPositions;
        private int positionIndex;

        private float shootTime;
        private float timeToShoot;

        private float damage;

        private int projectilesType;
        private int team;

        public bool IsShooting { get; set; }

        private ProjectilesController projectilesController;

        public Gun(GunInfo gunInfo, ProjectilesController pController, int team)
        {
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
        
        public void Update(Vector2 parentPos, Vector2 parentDir, float deltaTime)
        {
            if (timeToShoot <= 0f)
            {
                if (IsShooting)
                {
                    projectilesController.LaunchProjectile(Functions.RotateVector2(shootStartPositions[positionIndex], parentDir) + parentPos, parentDir, damage, team, projectilesType);
                    positionIndex = (positionIndex + 1) % shootStartPositions.Length;
                    timeToShoot = shootTime;
                }
            }
            else
                timeToShoot -= deltaTime;
        }
    }
}