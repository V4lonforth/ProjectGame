using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLib.Info;
using GameLib.GameObjects;

namespace GameLib.Controllers
{
    public abstract class BaseProjectilesController : IController
    {
        protected List<List<IProjectile>> activeProjectiles;
        protected List<List<IProjectile>> inactiveProjectiles;
        
        protected ProjectileInfo[] projectilesInfo;
        protected int projectilesTypesCount = 1;

        protected int projectilesStartCount;

        protected BaseProjectilesController()
        {
            activeProjectiles = new List<List<IProjectile>>();
            inactiveProjectiles = new List<List<IProjectile>>();
        }

        public BaseProjectilesController(int projectilesCount)
        {
            projectilesStartCount = projectilesCount;

            activeProjectiles = new List<List<IProjectile>>();
            inactiveProjectiles = new List<List<IProjectile>>();

            Texture2D[] projectilesSprites = new Texture2D[projectilesTypesCount];
            projectilesInfo = ProjectileInfo.GetProjectilesInfo(projectilesSprites);

            for (int i = 0; i < projectilesTypesCount; i++)
                CreateProjectiles(projectilesInfo[i], projectilesCount);
        }

        protected void CreateProjectiles(ProjectileInfo info, int count)
        {
            while (inactiveProjectiles.Count <= info.projectileType)
            {
                inactiveProjectiles.Add(new List<IProjectile>());
                activeProjectiles.Add(new List<IProjectile>());
            }
            for (int i = 0; i < count; i++)
                inactiveProjectiles[info.projectileType].Add(CreateProjectile(info));
        }
        private void RemoveProjectile(int projectileType, int index)
        {
            IProjectile projectile = activeProjectiles[projectileType][index];
            activeProjectiles[projectileType].RemoveAt(index);
            inactiveProjectiles[projectileType].Add(projectile);
            if (inactiveProjectiles[projectileType].Count > projectilesStartCount)
                inactiveProjectiles[projectileType].RemoveAt(inactiveProjectiles.Count - 1);
        }
        protected abstract IProjectile CreateProjectile(ProjectileInfo info);

        public virtual void LaunchProjectile(Vector2 pos, Vector2 dir, Vector2 velocity, float damage, int team, int projectileType)
        {
            if (inactiveProjectiles.Count <= projectileType || inactiveProjectiles[projectileType].Count == 0)
                CreateProjectiles(projectilesInfo[projectileType], 5);
            IProjectile projectile = inactiveProjectiles[projectileType][inactiveProjectiles.Count - 1];
            projectile.Launch(pos, dir, velocity, damage, team);
            activeProjectiles[projectileType].Add(projectile);
            inactiveProjectiles[projectileType].RemoveAt(inactiveProjectiles.Count - 1);
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < activeProjectiles.Count; i++)
            {
                for (int j = 0; j < activeProjectiles[i].Count; j++)
                {
                    if (activeProjectiles[i][j].IsDestroyed)
                    {
                        RemoveProjectile(i, j);
                        j--;
                    }
                    else
                        (activeProjectiles[i][j]).Update(deltaTime);
                }
            }
        }
    }
}