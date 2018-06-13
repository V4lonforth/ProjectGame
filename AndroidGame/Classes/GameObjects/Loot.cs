using Microsoft.Xna.Framework.Graphics;
using GameLib.Info;
using GameLib.GameObjects;
using GameLib.GameObjects.Base;
using AndroidGame.GameObjects.Base;

namespace AndroidGame.GameObjects
{
    class Loot : BaseLoot, ILoot, IDrawable
    {
        private Drawable drawable;

        public Loot(BodyInfo bodyInfo, Drawable draw) : base(bodyInfo)
        {
            drawable = draw;
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
            drawable.Position = Position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            drawable.Draw(spriteBatch);
        }
    }
}