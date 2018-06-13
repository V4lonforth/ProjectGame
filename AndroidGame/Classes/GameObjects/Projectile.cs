using AndroidGame.Controllers;
using AndroidGame.GameObjects.Base;
using GameLib.GameObjects;
using GameLib.GameObjects.Base;
using GameLib.Info;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.GameObjects
{
    public class Projectile : BaseProjectile, IProjectile, GameLib.GameObjects.Base.IDrawable
    {
        private ParticleSpawner particleSpawner;
        private Drawable drawable;

        public Projectile(ProjectileInfo info, ParticleSystem particleSystem) : base(info)
        {
            particleSpawner = new ParticleSpawner(particleSystem, this, info.spawnerInfo);
            drawable = new Drawable(info.texture, size: info.spriteSize, col: info.color);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            drawable.Draw(spriteBatch);
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
            particleSpawner.Update(deltaTime);
            drawable.Position = Position;
            drawable.Rotation = (float)System.Math.Atan2(LookingDirection.Y, LookingDirection.X);
        }
    }
}
