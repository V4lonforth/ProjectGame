using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.GameObjects.Base
{
    class Drawable
    {
        private Texture2D texture;
        private Vector2 centre;

        protected Rectangle destinationRectangle;

        private Color color;
        private float depth;

        public Vector2 Position
        {
            set
            {
                destinationRectangle.X = (int)value.X;
                destinationRectangle.Y = (int)value.Y;
            }
        }
        public float Rotation
        {
            protected get;
            set;
        }

        private SpriteEffects spriteEffects;

        public Drawable(Texture2D text, Vector2 position, Vector2? size = null, Color? col = null, float rot = 0f, float dep = 0.5f, SpriteEffects effect = SpriteEffects.None)
        {
            texture = text;
            destinationRectangle.X = (int)position.X;
            destinationRectangle.Y = (int)position.Y;
            if (size.HasValue)
            {
                destinationRectangle.Width = (int)size.Value.X;
                destinationRectangle.Height = (int)size.Value.Y;
            }
            else
            {
                destinationRectangle.Width = text.Width;
                destinationRectangle.Height = text.Height;
            }
            if (col.HasValue)
                color = col.Value;
            else
                color = Color.White;
            centre = text.Bounds.Size.ToVector2() / 2f;
            Rotation = rot;
            depth = dep;
            spriteEffects = effect;
        }
        public Drawable(Texture2D text, Rectangle destRect, Color? col = null, float rot = 0f, float dep = 0.5f, SpriteEffects effect = SpriteEffects.None)
        {
            texture = text;
            destinationRectangle = destRect;
            if (col.HasValue)
                color = col.Value;
            else
                color = Color.White;
            centre = text.Bounds.Size.ToVector2() / 2f;
            Rotation = rot;
            depth = dep;
            spriteEffects = effect;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destinationRectangle, null, color, Rotation, centre, spriteEffects, depth);
        }
    }
}