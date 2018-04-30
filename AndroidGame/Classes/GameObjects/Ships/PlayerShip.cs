using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Controllers;
using AndroidGame.GameObjects.Base;
using AndroidGame.Serialization;
using NetworkLib;

namespace AndroidGame.GameObjects.Ships
{
    class PlayerShip : Ship, IPhysicalObject
    {
        private Connection connection;

        public float CurrentExperience { get; private set; }
        public float NextLevelExperience { get; private set; }

        public PlayerShip(ShipInfo shipInfo, Texture2D[] shipPartSprites, ProjectilesController projController, ParticleSystem parSystem, int team, Vector2 pos, Connection connect) 
            : base(shipInfo, shipPartSprites, projController, parSystem, team, pos)
        {
            connection = connect;
        }

        public void GainExperience(float exp)
        {
            CurrentExperience += exp;
            if (CurrentExperience >= NextLevelExperience)
            {
                CurrentExperience -= NextLevelExperience;
                GainLevel();
            }
        }
        private void GainLevel()
        {

        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);

            connection.SendShipData(new ShipData[]
                {
                    new ShipData()
                    {
                        lookingDirection = LookingDirection,
                        movementDirection = MovementDirection,
                        position = Position,
                        speed = Speed,
                        time = deltaTime
                    }
                });
        }
    }
}