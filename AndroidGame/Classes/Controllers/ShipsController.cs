using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AndroidGame.GameObjects.Ships;
using AndroidGame.Net;
using AndroidGame.GUI;
using GameLib.Info;
using GameLib.Geometry;
using GameLib.GameObjects.Base;
using GameLib.Controllers;

namespace AndroidGame.Controllers
{
    class ShipsController : IDrawablesController
    {
        private BaseLootController lootController;
        private BaseProjectilesController projectilesController;
        private ParticleSystem particleSystem;

        private Camera camera;

        private Action<Action<Vector2>, Action<Vector2>> joystickActions;

        private List<IPhysicalObject> ships;
        private List<IPhysicalObject> shipsPlayers;

        private const float minSpawnDistance = 300f;
        private const float maxSpawnDistance = 500f;

        private ShipInfo[] shipsInfo;
        private const int shipsTypesCount = 2;

        private const string shipSpritesPath = "Sprites/Ships/Ship";

        public ShipsController(ContentManager Content, BaseLootController lController, BaseProjectilesController projController, ParticleSystem parSystem, 
            Camera cam, Action<Action<Vector2>, Action<Vector2>> actions)
        {
            joystickActions = actions;
            camera = cam;
            Texture2D[] shipSprites = new Texture2D[shipsTypesCount];
            for (int i = 0; i < shipsTypesCount; i++)
                shipSprites[i] = Content.Load<Texture2D>(shipSpritesPath + i.ToString());
            shipsInfo = ShipInfo.GetShipsInfo(shipSprites);

            ships = new List<IPhysicalObject>();
            shipsPlayers = new List<IPhysicalObject>();
            lootController = lController;
            projectilesController = projController;
            particleSystem = parSystem;
        }

        public ShipController CreatePlayerShip(Sender sender, Vector2 pos, int id, int shipType = 0, int team = 1)
        {
            ShipController shipController = new ShipController();
            PlayerShip playerShip = new PlayerShip(shipsInfo[shipType], projectilesController, particleSystem, team, pos, id, sender, shipController);
            shipController.SetShip(playerShip);
            AddShip(playerShip);
            camera.Target = playerShip;
            joystickActions(playerShip.SetMovementDirection, playerShip.SetAttackDirection);
            return shipController;
        }

        public void RemoveShip(Ship ship)
        {
            ships.Remove(ship);
            if (ship.GetType() == typeof(EnemyPlayerShip))
                shipsPlayers.Remove(ship);
        }
        public void AddShip(Ship ship)
        {
            ships.Add(ship);
            if (ship.GetType() == typeof(EnemyPlayerShip))
                shipsPlayers.Add(ship);
        }

        public void CreateAIShip(int type, int id)
        {
            foreach (Ship ship in shipsPlayers)
            {
                Vector2 position = Functions.RandomVector2(minSpawnDistance, maxSpawnDistance) + ship.Position;
                AIShip newShip = new AIShip(shipsInfo[type], projectilesController, particleSystem, 0, position, id);
                AddShip(newShip);
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                Ship ship = (Ship)ships[i];
                if (ship.IsDestroyed)
                {
                    ships.RemoveAt(i);
                    i--;
                    lootController.AddLoot(ship.DroppingExperience, ship.Position);
                }
                else
                    ships[i].Update(deltaTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ship ship in ships)
                ship.Draw(spriteBatch);
        }
    }
}