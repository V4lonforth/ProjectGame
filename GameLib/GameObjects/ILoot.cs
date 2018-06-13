using Microsoft.Xna.Framework;
using GameLib.GameObjects.Base;

namespace GameLib.GameObjects
{
    public interface ILoot : IPhysicalObject
    {
        bool IsActive
        {
            get;
        }

        void Launch(float exp, Vector2 pos, Vector2 dir);
    }
}
