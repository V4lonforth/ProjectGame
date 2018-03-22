﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Serialization;
using AndroidGame.Geometry;
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

        private List<Ship> enemies;
        
        public AIShip(ShipInfo shipInfo, Texture2D[] shipPartSprites, ProjectilesController pController, int team, Vector2 pos, Vector2 dir, List<Ship> ships) : base(shipInfo, shipPartSprites, pController, team, pos, dir)
        {
            enemies = ships;
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