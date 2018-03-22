using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.GameObjects.Base
{
    interface IPhysicalObject
    {
        void Update(float deltaTime);
        void Draw(SpriteBatch spriteBatch);
    }
}