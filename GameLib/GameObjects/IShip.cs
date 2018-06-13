using GameLib.GameObjects.Base;
using Microsoft.Xna.Framework;

namespace GameLib.GameObjects
{
    public interface IShip : IPhysicalObject
    {
        int Id { get; }
        void SetMovementDirection(Vector2 dir);
        void SetAttackDirection(Vector2 dir);
        void GainExperience(float exp);
    }
}
