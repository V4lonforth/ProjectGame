using GameLib.GameObjects.Base;
using Microsoft.Xna.Framework;

namespace GameLib.GameObjects
{
    public interface IProjectile : IPhysicalObject
    {
        bool IsActive { get; }
        void Launch(Vector2 pos, Vector2 dir, Vector2 velocity, float damage, int team);
    }
}
