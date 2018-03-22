using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AndroidGame.GameObjects.Ships;
using AndroidGame.Serialization;
using AndroidGame.Geometry;

namespace AndroidGame.Controllers
{
    class ShipsController : IController
    {
        private LootController lootController;
        private ProjectilesController projectilesController;

        private List<Ship> ships;
        private List<Ship> shipsPlayers;

        private const float minSpawnDistance = 300f;
        private const float maxSpawnDistance = 500f;

        private ShipInfo[] shipsInfo;
        private Texture2D[] shipSprites;

        private const string shipSpritesPath = "Sprites/Ships/Ship";

        public ShipsController(ContentManager Content, SerializationManager serializationManager, LootController lController, ProjectilesController pController)
        {
            shipsInfo = serializationManager.LoadInfo<ShipInfo>("Ships");
            shipSprites = new Texture2D[shipsInfo.Length];
            for (int i = 0; i < shipSprites.Length; i++)
                shipSprites[i] = Content.Load<Texture2D>(shipSpritesPath + i.ToString());
            ships = new List<Ship>();
            shipsPlayers = new List<Ship>();
            lootController = lController;
            projectilesController = pController;
        }

        public PlayerShip CreatePlayerShip(Vector2 pos, Vector2 dir, int shipType = 0, int team = 1)
        {
            PlayerShip playerShip = new PlayerShip(shipsInfo[shipType], shipSprites, projectilesController, team, pos, dir);
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
                Vector2 direction = Functions.RandomVector2();
                AIShip newShip = new AIShip(shipsInfo[type], shipSprites, projectilesController, 0, position, direction, shipsPlayers);
                AddShip(newShip);
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i].IsDestroyed)
                {
                    ships.RemoveAt(i);
                    i--;
                    lootController.AddLoot(ships[i].DroppingExperience, ships[i].Position);
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