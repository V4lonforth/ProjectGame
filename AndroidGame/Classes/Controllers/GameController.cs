using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameLib.Controllers;
using AndroidGame.GUI;
using AndroidGame.Net;

namespace AndroidGame.Controllers
{
    class GameController : IDrawablesController
    {
        private IDrawablesController[] controllers;
        private GUIController GUIController;
        
        private ParticleSystem particleSystem;
        private NetController netController;

        private Camera camera;

        public GameController(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            camera = new Camera(GUIController.screenSize);

            particleSystem = new ParticleSystem(Content, camera, graphicsDevice);
            ProjectilesController projectilesController = new ProjectilesController(Content, particleSystem);
            LootController lootController = new LootController(Content);
            GUIController = new GUIController(Content);
            ShipsController shipsController = new ShipsController(Content, lootController, projectilesController, particleSystem, camera, GUIController.CreateJoysticks);
            netController = new NetController(shipsController);

            controllers = new IDrawablesController[]
            {
                shipsController,
                projectilesController,
                lootController
            };
        }

        public void Update(float deltaTime)
        {
            GUIController.Update(deltaTime);

            particleSystem.Update(deltaTime);

            camera.Update(deltaTime);

            foreach (IController controller in controllers)
                controller.Update(deltaTime);

            netController.Update();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: camera.TransformMatrix);

            particleSystem.Draw();
            foreach (IDrawablesController controller in controllers)
                controller.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            GUIController.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}