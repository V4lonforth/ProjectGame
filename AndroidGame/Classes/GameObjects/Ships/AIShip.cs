using Microsoft.Xna.Framework;
using GameLib.Info;
using GameLib.Geometry;
using GameLib.Controllers;
using AndroidGame.Controllers;

namespace AndroidGame.GameObjects.Ships
{
    class AIShip : Ship
    {
        private enum ActionState
        {
            Moving,
            Attacking,
            TacticalRetreating
        }

        private ActionState state;

        private Vector2 targetDirection;
        private float rotationspeed;
        private float actionTimeLeft;
        
        public AIShip(ShipInfo shipInfo, BaseProjectilesController projController, ParticleSystem parSystem, int team, Vector2 pos, int id) 
            : base(shipInfo, projController, parSystem, team, pos, id)
        {
            state = ActionState.Moving;
            StartMoving();
        }

        public new void Update(float deltaTime)
        {
            actionTimeLeft -= deltaTime;
            switch (state)
            {
                case ActionState.Moving:
                    MovementDirection = Functions.CircleLerp(LookingDirection, targetDirection, rotationspeed * deltaTime);
                    if (actionTimeLeft <= 0f)
                        StartMoving();
                    break;
            }
            base.Update(deltaTime);
        }
        
        private void StartMoving()
        {
            targetDirection = Functions.RandomVector2();
            rotationspeed = (float)Functions.random.NextDouble() / 3f;
            actionTimeLeft = (float)Functions.random.NextDouble() * 5f + 5f;
        }
    }
}