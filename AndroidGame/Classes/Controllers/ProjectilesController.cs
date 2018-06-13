using AndroidGame.GameObjects;
using GameLib.Controllers;
using GameLib.GameObjects;
using GameLib.Info;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.Controllers
{
    class ProjectilesController : BaseProjectilesController, IDrawablesController
    {
        private ParticleSystem particleSystem;

        private const string projectileSpritesPath = "Sprites/Projectiles/Projectile";

        public ProjectilesController(ContentManager Content, ParticleSystem partSystem, int projectilesCount = 50) : base()
        {
            projectilesStartCount = projectilesCount;
            particleSystem = partSystem;

            Texture2D[] projectilesSprites = new Texture2D[projectilesTypesCount];
            for (int i = 0; i < projectilesTypesCount; i++)
                projectilesSprites[i] = Content.Load<Texture2D>(projectileSpritesPath + i.ToString());
            projectilesInfo = ProjectileInfo.GetProjectilesInfo(projectilesSprites);

            for (int i = 0; i < projectilesTypesCount; i++)
                CreateProjectiles(projectilesInfo[i], projectilesStartCount);
        }

        protected override IProjectile CreateProjectile(ProjectileInfo info)
        {
            return new Projectile(info, particleSystem);
        }

        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < activeProjectiles.Count; i++)
                for (int j = 0; j < activeProjectiles[i].Count; j++)
                ((Projectile)activeProjectiles[i][j]).Draw(spriteBatch);
        }
    }
}