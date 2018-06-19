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
using GameLib.GameObjects;

namespace AndroidGame.Controllers
{
    class ShipsController : IDrawablesController
    {
        private BaseLootController lootController;
        private BaseProjectilesController projectilesController;
        private ParticleSystem particleSystem;

        private Camera camera;

        private Action<Action<Vector2>, Action<Vector2>> joystickActions;

        private List<ShipController> ships;
        private List<ShipController> shipsPlayers;

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

            ships = new List<ShipController>();
            shipsPlayers = new List<ShipController>();
            lootController = lController;
            projectilesController = projController;
            particleSystem = parSystem;
        }
        private BaseShip CreateTestShip(Vector2 pos, int shipType)
        {
            return new BaseShip(shipsInfo[shipType], null, 0, pos, 0, false);
        }
        public ShipController CreateEnemyPlayerShip(Vector2 pos, int id, int shipType = 0, int team = 1)
        {
            ShipController shipController = new ShipController();
            EnemyPlayerShip enemyPlayerShip = new EnemyPlayerShip(shipsInfo[shipType], projectilesController, particleSystem, team, pos, id);
            shipController.SetShip(enemyPlayerShip, CreateTestShip(pos, shipType));
            AddShip(shipController);
            return shipController;
        }
        public ShipController CreatePlayerShip(Sender sender, Vector2 pos, int id, int shipType = 0, int team = 1)
        {
            ShipController shipController = new ShipController();
            PlayerShip playerShip = new PlayerShip(shipsInfo[shipType], projectilesController, particleSystem, team, pos, id, sender, shipController);
            shipController.SetShip(playerShip, CreateTestShip(pos, shipType));
            AddShip(shipController);
            camera.Target = playerShip;
            joystickActions(playerShip.SetMovementDirection, playerShip.SetAttackDirection);
            return shipController;
        }

        public void RemoveShip(ShipController ship)
        {
            ships.Remove(ship);
            if (ship.GetType() == typeof(EnemyPlayerShip))
                shipsPlayers.Remove(ship);
            if (ship.GetType() == typeof(PlayerShip))
            {
                shipsPlayers.Remove(ship);
                joystickActions((v) => { }, (v) => { });
            }
        }
        public void AddShip(ShipController ship)
        {
            ships.Add(ship);
            if (ship.GetType() == typeof(EnemyPlayerShip) || ship.GetType() == typeof(PlayerShip))
                shipsPlayers.Add(ship);
        }

        public ShipController CreateAIShip(Vector2 pos, int id, int shipType = 0, int team = 0)
        {
            ShipController shipController = new ShipController();
            AIShip ship = new AIShip(shipsInfo[shipType], projectilesController, particleSystem, team, pos, id);
            shipController.SetShip(ship, CreateTestShip(pos, shipType));
            AddShip(shipController);
            return shipController;
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                ShipController ship = ships[i];
                /*if (ship.IsDestroyed)
                {
                    ships.RemoveAt(i);
                    i--;
                    lootController.AddLoot(ship.DroppingExperience, ship.Position);
                }
                else*/
                ships[i].Update();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ShipController ship in ships)
                ((GameLib.GameObjects.Base.IDrawable)ship.Ship).Draw(spriteBatch);
        }
    }
}