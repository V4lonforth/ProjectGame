using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Controllers;
using AndroidGame.Serialization;

namespace AndroidGame.GameObjects.Ships
{
    class PlayerShip : Ship
    {

        public float CurrentExperience { get; private set; }
        public float NextLevelExperience { get; private set; }

        public PlayerShip(ShipInfo shipInfo, Texture2D[] shipPartSprites, ProjectilesController pController, int team, Vector2 pos, Vector2 dir) 
            : base(shipInfo, shipPartSprites, pController, team, pos, dir)
        {

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