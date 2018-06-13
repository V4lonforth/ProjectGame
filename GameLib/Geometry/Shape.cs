using Microsoft.Xna.Framework;
using System;

namespace GameLib.Geometry
{
    public abstract class Shape : ICloneable
    {
        public abstract object Clone();

        public abstract void Move(Shape baseShape, Vector2 offset);
        public abstract void Rotate(Shape baseShape, Vector2 dir);

        public abstract void ChangeSize(float multiplier);
        
        public abstract float GetMaxDistance();
    }
}
