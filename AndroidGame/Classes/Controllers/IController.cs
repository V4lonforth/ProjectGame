using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.Controllers
{
    public interface IController
    {
        void Update(float deltaTime);
        void Draw(SpriteBatch spriteBatch);
    }
}