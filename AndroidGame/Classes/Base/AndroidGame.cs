using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AndroidGame.Controllers;

namespace AndroidGame
{
    public class AndroidGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private IController controller;

        public AndroidGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            controller = new GameController(Content, GraphicsDevice);
        }
        
        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            controller.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            controller.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
