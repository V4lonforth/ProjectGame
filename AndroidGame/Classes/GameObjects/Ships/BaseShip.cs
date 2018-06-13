using System;
using AndroidGame.GameObjects.Base;
using AndroidGame.Physics;
using AndroidGame.Serialization;
using Microsoft.Xna.Framework;
using NetworkLib.Data;

namespace AndroidGame.GameObjects.Ships
{
    public class BaseShip : PhysicalObjectAcc
    {
        protected float rotationSpeed;
        protected float health;

        public int Team { get; private set; }
        public bool IsDestroyed { get; private set; }

        public float DroppingExperience { get; protected set; }

        public BaseShip(ShipInfo shipInfo, Drawable draw, int team, Vector2 pos)
            : base(draw, shipInfo.maxSpeed, shipInfo.acceleration, PhysicalObjectType.Ship, pos, Vector2.UnitX, Vector2.UnitX, shipInfo.bodyInfo)
        {
            health = shipInfo.health;
            rotationSpeed = shipInfo.rotationSpeed;
            IsDestroyed = false;
            Team = team;
        }

        

        private void Destroy()
        {
            IsDestroyed = true;
        }

        protected override bool OnCollision(Body body)
        {
            switch (body.Parent.Type)
            {
                case PhysicalObjectType.Projectile:
                    if (((Projectile)body.Parent).Team != Team)
                    {
                        health -= ((Projectile)body.Parent).Damage;
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
    }
}