using AndroidGame.GameObjects;
using GameLib.Controllers;
using GameLib.GameObjects;
using GameLib.Info;
using AndroidGame.GameObjects.Base;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.Controllers
{
    class LootController : BaseLootController, IDrawablesController
    {
        private const string lootSpritePath = "Sprites/Loot/Loot";

        private Texture2D lootSprite;

        public LootController(ContentManager Content) : base()
        {
            lootSprite = Content.Load<Texture2D>(lootSpritePath);
            FillLoot();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Loot loot in activeLootList)
                loot.Draw(spriteBatch);
        }

        protected override ILoot CreateLoot(BodyInfo bodyInfo)
        {
            return new Loot(bodyInfo, new Drawable(lootSprite));
        }
    }
}