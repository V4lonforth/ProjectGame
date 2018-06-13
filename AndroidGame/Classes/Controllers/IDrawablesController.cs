using GameLib.Controllers;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.Controllers
{
    public interface IDrawablesController : IController
    {
        void Draw(SpriteBatch spriteBatch);
    }
}