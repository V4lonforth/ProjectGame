using Microsoft.Xna.Framework;
using GameLib.Info;
using GameLib.Controllers;
using GameLib.Physics;
using GameLib.Geometry;
using GameLib.GameObjects.Base;
using NetworkLib.Data;

namespace GameLib.GameObjects
{
    public class BaseShip : PhysicalObjectAcc, IShip
    {
        private Gun gun;

        private float timeToRotateLeft;
        private const float timeToRotate = 0.35f;

        protected float rotationSpeed;
        protected float health;

        public int ShipType { get; private set; }
        public int Team { get; private set; }
        public bool IsDestroyed { get; private set; }

        public float DroppingExperience { get; protected set; }

        public float CurrentExperience { get; private set; }
        public float NextLevelExperience { get; private set; }

        public int Id { get; private set; }

        public BaseShip(ShipInfo shipInfo, BaseProjectilesController projController, int team, Vector2 pos, int id, bool isActive)
            : base(shipInfo.maxSpeed, shipInfo.acceleration, PhysicalObjectType.Ship, pos, Vector2.UnitX, Vector2.UnitX, shipInfo.bodyInfo, isActive)
        {
            ShipType = shipInfo.shipType;
            health = shipInfo.health;
            rotationSpeed = shipInfo.rotationSpeed;
            IsDestroyed = false;
            Team = team;
            gun = new Gun(this, shipInfo.gunInfo, projController, team, isActive);
            rotationSpeed = shipInfo.rotationSpeed;
            Id = id;
        }

        public void SetMovementDirection(Vector2 dir)
        {
            if (dir == Vector2.Zero)
                isAccelerating = false;
            else
            {
                isAccelerating = true;
                MovementDirection = dir;
            }
        }
        public void SetAttackDirection(Vector2 dir)
        {
            if (dir == Vector2.Zero)
            {
                gun.IsShooting = false;
            }
            else
            {
                timeToRotateLeft = timeToRotate;
                LookingDirection = dir;
                gun.IsShooting = true;
            }
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
        protected void GainLevel()
        {

        }

        protected void Destroy()
        {
            IsDestroyed = true;
        }

        protected override bool OnCollision(Body body)
        {
            PhysicalObject parent = (PhysicalObject)body.Parent;
            switch (parent.Type)
            {
                case PhysicalObjectType.Projectile:
                    if (((BaseProjectile)body.Parent).Team != Team)
                    {
                        health -= ((BaseProjectile)body.Parent).Damage;
                        if (health <= 0f)
                        {
                            Destroy();
                            return true;
                        }
                    }
                    return false;
            }
            return false;
        }

        public new void Update(float deltaTime)
        {
            base.Update(deltaTime);
            gun.Update(deltaTime);
            if (!gun.IsShooting)
            {
                timeToRotateLeft -= deltaTime;
                if (timeToRotateLeft <= 0f && isAccelerating)
                    LookingDirection = Functions.CircleLerp(LookingDirection, MovementDirection, rotationSpeed * deltaTime);
            }
        }
        public ShipData GetShipData()
        {
            return new ShipData()
            {
                lookingDirection = LookingDirection,
                speed = Speed,
                movementDirection = MovementDirection,
                position = Position,
                health = health,
                timeToRotate = timeToRotateLeft,
                timeToShoot = gun.timeToShoot
            };
        }
        public void SetInput(ref InputData inputData)
        {
            SetMovementDirection(inputData.movementDirection);
            SetAttackDirection(inputData.attackDirection);
        }
        public void SetShipState(ref ShipData shipData, ref InputData inputData)
        {
            Position = shipData.position;
            LookingDirection = shipData.lookingDirection;
            MovementDirection = shipData.movementDirection;
            Speed = shipData.speed;
            timeToRotateLeft = shipData.timeToRotate;
            gun.timeToShoot = shipData.timeToShoot;

            SetInput(ref inputData);
        }
    }
}