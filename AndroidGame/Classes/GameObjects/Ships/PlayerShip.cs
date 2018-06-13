using Microsoft.Xna.Framework;
using AndroidGame.Net;
using AndroidGame.Controllers;
using GameLib.Info;
using GameLib.Controllers;
using GameLib.GameObjects;
using NetworkLib.Data;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.GameObjects.Ships
{
    class PlayerShip : Ship, IShip
    {
        private ShipController shipController;
        private Sender sender;

        private InputData inputData;
        private TimeData timeData;

        public PlayerShip(ShipInfo shipInfo, BaseProjectilesController projController, ParticleSystem parSystem, int team, Vector2 pos, int id, Sender send, ShipController shipController) 
            : base(shipInfo, projController, parSystem, team, pos, id)
        {
            this.shipController = shipController;
            sender = send;
            timeData = new TimeData()
            {
                dataNumber = 1,
                time = AndroidGame.time
            };
        }

        public new void SetMovementDirection(Vector2 dir)
        {
            base.SetMovementDirection(dir);
            inputData.movementDirection = dir;
        }
        public new void SetAttackDirection(Vector2 dir)
        {
            base.SetAttackDirection(dir);
            inputData.attackDirection = dir;
        }
        public new void GainExperience(float exp)
        {
            base.GainExperience(exp);
        }

        public void CreateShip(ShipInfo shipInfo, Vector2 position)
        {
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
            timeData.dataNumber++;
            timeData.time = AndroidGame.time;
            shipController.AddInputData(ref timeData, ref inputData);
            sender.Add(DataType.Time, timeData);
            sender.Add(DataType.Input, inputData);
            sender.Send();
        }
    }
}