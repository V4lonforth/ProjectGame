using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AndroidGame.GameObjects.Ships;
using AndroidGame.Serialization;
using AndroidGame.Geometry;
using AndroidGame.GameObjects.Base;
using NetworkLib;

namespace AndroidGame.Controllers
{
    class ShipsController : IController
    {
        private LootController lootController;
        private ProjectilesController projectilesController;
        private ParticleSystem particleSystem;

        private List<IPhysicalObject> ships;
        private List<IPhysicalObject> shipsPlayers;

        private const float minSpawnDistance = 300f;
        private const float maxSpawnDistance = 500f;

        private ShipInfo[] shipsInfo;
        private Texture2D[] shipSprites;

        private const string shipSpritesPath = "Sprites/Ships/Ship";

        public ShipsController(ContentManager Content, SerializationManager serializationManager, LootController lController, ProjectilesController projController, ParticleSystem parSystem)
        {
            shipsInfo = serializationManager.LoadInfo<ShipInfo>("Ships");
            shipSprites = new Texture2D[shipsInfo.Length];
            for (int i = 0; i < shipSprites.Length; i++)
                shipSprites[i] = Content.Load<Texture2D>(shipSpritesPath + i.ToString());
            ships = new List<IPhysicalObject>();
            shipsPlayers = new List<IPhysicalObject>();
            lootController = lController;
            projectilesController = projController;
            particleSystem = parSystem;
        }

        public PlayerShip CreatePlayerShip(Vector2 pos, int shipType = 1, int team = 1)
        {
            PlayerShip playerShip = new PlayerShip(shipsInfo[shipType], shipSprites, projectilesController, particleSystem, team, pos);
            AddShip(playerShip);
            return playerShip;
        }

        public void RemoveShip(Ship ship)
        {
            ships.Remove(ship);
            if (ship.GetType() == typeof(PlayerShip))
                shipsPlayers.Remove(ship);
        }
        public void AddShip(Ship ship)
        {
            ships.Add(ship);
            if (ship.GetType() == typeof(PlayerShip))
                shipsPlayers.Add(ship);
        }

        public void CreateAIShip(int type)
        {
            foreach (Ship ship in shipsPlayers)
            {
                Vector2 position = Functions.RandomVector2(minSpawnDistance, maxSpawnDistance) + ship.Position;
                AIShip newShip = new AIShip(shipsInfo[type], shipSprites, projectilesController, particleSystem, 0, position, shipsPlayers);
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