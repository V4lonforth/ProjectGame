using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AndroidGame.Serialization;
using AndroidGame.GameObjects.Ships;
using AndroidGame.GUI;
using NetworkLib;

namespace AndroidGame.Controllers
{
    class GameController : IController
    {
        private IController[] controllers;
        private GUIController GUIController;

        private ParticleSystem particleSystem;
        private Camera camera;

        public GameController(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            SerializationManager serializationManager = new SerializationManager();

            camera = new Camera(GUIController.screenSize);
            particleSystem = new ParticleSystem(Content, camera, graphicsDevice);

            ProjectilesController projectilesController = new ProjectilesController(Content, serializationManager, particleSystem);
            LootController lootController = new LootController(Content);
            ShipsController shipsController = new ShipsController(Content, serializationManager, lootController, projectilesController, particleSystem);
            PlayerShip playerShip = shipsController.CreatePlayerShip(Vector2.One * 250);
            camera.Target = playerShip;
            GUIController = new GUIController(Content, playerShip.SetMovementDirection, playerShip.SetAttackDirection);


            controllers = new IController[]
            {
                projectilesController,
                lootController,
                shipsController
            };
        }

        public void Update(float deltaTime)
        {
            particleSystem.Update(deltaTime);

            camera.Update(deltaTime);

            foreach (IController controller in controllers)
                controller.Update(deltaTime);

            GUIController.Update(deltaTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: camera.TransformMatrix);

            particleSystem.Draw();
            foreach (IController controller in controllers)
                controller.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            GUIController.Draw(spriteBatch);
            spriteBatch.End();

        }
    }
}