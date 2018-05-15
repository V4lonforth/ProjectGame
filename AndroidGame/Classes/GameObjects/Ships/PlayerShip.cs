using System;
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
        public float CurrentExperience { get; private set; }
        public float NextLevelExperience { get; private set; }

        private int gameTime ;

        public PlayerShip(ShipInfo shipInfo, Texture2D[] shipPartSprites, ProjectilesController projController, ParticleSystem parSystem, int team, Vector2 pos) 
            : base(shipInfo, shipPartSprites, projController, parSystem, team, pos)
        {
            gameTime = 0;
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
    }
}