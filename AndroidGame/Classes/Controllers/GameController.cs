using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AndroidGame.Serialization;
using AndroidGame.GameObjects.Ships;

namespace AndroidGame.Controllers
{
    class GameController : IController
    {
        private IController[] controllers;
        
        public GameController(ContentManager Content)
        {
            SerializationManager serializationManager = new SerializationManager();

            ProjectilesController projectilesController = new ProjectilesController(Content, serializationManager);
            LootController lootController = new LootController(Content);
            ShipsController shipsController = new ShipsController(Content, serializationManager, lootController, projectilesController);
            PlayerShip playerShip = shipsController.CreatePlayerShip(Vector2.One * 250, Vector2.UnitX);
            GUIController GUIController = new GUIController(Content, playerShip.SetMovementDirection, playerShip.SetAttackDirection);

            controllers = new IController[]
            {
                projectilesController,
                lootController,
                shipsController,
                GUIController
            };
        }

        public void Update(float deltaTime)
        {
            foreach (IController controller in controllers)
                controller.Update(deltaTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IController controller in controllers)
                controller.Draw(spriteBatch);
        }
    }
}