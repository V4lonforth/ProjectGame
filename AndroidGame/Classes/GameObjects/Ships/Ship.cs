using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLib.Info;
using AndroidGame.Controllers;
using GameLib.GameObjects.Base;
using GameLib.GameObjects;
using GameLib.Controllers;
using AndroidGame.GameObjects.Base;

namespace AndroidGame.GameObjects.Ships
{
    public class Ship : BaseShip, IShip, GameLib.GameObjects.Base.IDrawable
    {
        private ParticleSpawner particleSpawner;
        private Drawable drawable;

        public Ship(ShipInfo shipInfo, BaseProjectilesController projController, ParticleSystem parSystem, int team, Vector2 pos, int id) : base(shipInfo, projController, team, pos, id, true)
        {
            particleSpawner = new ParticleSpawner(parSystem, this, shipInfo.spawnerInfo);
            drawable = new Drawable(shipInfo.texture, size: shipInfo.spriteSize, col: shipInfo.color);
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