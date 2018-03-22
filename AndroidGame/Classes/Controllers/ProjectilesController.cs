using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AndroidGame.GameObjects;
using AndroidGame.Serialization;

namespace AndroidGame.Controllers
{
    class ProjectilesController : IController
    {
        private List<List<Projectile>> activeProjectiles;
        private List<List<Projectile>> inactiveProjectiles;

        private Texture2D[] projectilesSprites;
        private ProjectileInfo[] projectilesInfo;

        private const string projectileSpritesPath = "Sprites/Projectiles/Projectile";

        private const int projectilesStartCount = 30;

        public ProjectilesController(ContentManager Content, SerializationManager serializationManager)
        {
            activeProjectiles = new List<List<Projectile>>();
            inactiveProjectiles = new List<List<Projectile>>();
            projectilesInfo = serializationManager.LoadInfo<ProjectileInfo>("Projectiles");
            projectilesSprites = new Texture2D[projectilesInfo.Length];

            for (int i = 0; i < projectilesSprites.Length; i++)
                projectilesSprites[i] = Content.Load<Texture2D>(projectileSpritesPath + i.ToString());
            for (int i = 0; i < projectilesInfo.Length; i++)
                CreateProjectiles(projectilesInfo[i], projectilesStartCount);
        }

        private void CreateProjectiles(ProjectileInfo info, int count)
        {
            while (inactiveProjectiles.Count <= info.projectileType)
            {
                inactiveProjectiles.Add(new List<Projectile>());
                activeProjectiles.Add(new List<Projectile>());
            }
            for (int i = 0; i < count; i++)
                inactiveProjectiles[info.projectileType].Add(new Projectile(info, projectilesSprites));
        }
        private void RemoveProjectile(int projectileType, int index)
        {
            Projectile projectile = activeProjectiles[projectileType][index];
            activeProjectiles[projectileType].RemoveAt(index);
            inactiveProjectiles[projectileType].Add(projectile);
            if (inactiveProjectiles[projectileType].Count > projectilesStartCount)
                inactiveProjectiles[projectileType].RemoveAt(inactiveProjectiles.Count - 1);
        }

        public void LaunchProjectile(Vector2 pos, Vector2 dir, float damage, int team, int projectileType)
        {
            if (inactiveProjectiles.Count <= projectileType || inactiveProjectiles[projectileType].Count == 0)
                CreateProjectiles(projectilesInfo[projectileType], 5);
            Projectile projectile = inactiveProjectiles[projectileType][inactiveProjectiles.Count - 1];
            projectile.Launch(pos, dir, damage, team);
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
                        activeProjectiles[i][j].Update(deltaTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (List<Projectile> projectiles in activeProjectiles)
                foreach (Projectile projectile in projectiles)
                    projectile.Draw(spriteBatch);
        }
    }
}