using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System;

namespace AndroidGame.Geometry
{
    [XmlRoot("Shape")]
    [XmlInclude(typeof(Circle)), XmlInclude(typeof(Polygon))]
    public abstract class Shape : ICloneable
    {
        public abstract object Clone();

        public abstract void Move(Shape baseShape, Vector2 offset);
        public abstract void Rotate(Shape baseShape, Vector2 dir);

        public abstract void ChangeSize(float multiplier);
        
        public abstract float GetMaxDistance();
    }
}
