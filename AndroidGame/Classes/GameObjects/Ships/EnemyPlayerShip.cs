using Microsoft.Xna.Framework;
using AndroidGame.Controllers;
using GameLib.Info;
using GameLib.Controllers;

namespace AndroidGame.GameObjects.Ships
{
    class EnemyPlayerShip : Ship
    {
        
        public EnemyPlayerShip(ShipInfo shipInfo, BaseProjectilesController projController, ParticleSystem parSystem, int team, Vector2 pos, int id) 
            : base(shipInfo, projController, parSystem, team, pos, id)
        {
        }
        
        public new void SetMovementDirection(Vector2 dir)
        {
            base.SetMovementDirection(dir);
            
        }
        public new void SetAttackDirection(Vector2 dir)
        {
            base.SetAttackDirection(dir);
        }
    }
}