using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using AndroidGame.Physics;
using AndroidGame.Geometry;

namespace AndroidGame.Serialization
{
    [XmlRoot("Projectile")]
    public class ProjectileInfo : ISerializationInfo
    {
        [XmlElement("Damage")]
        public float damage;

        [XmlElement("Speed")]
        public float speed;

        [XmlElement("SpriteSize")]
        public Vector2 spriteSize;

        [XmlElement("SpriteIndex")]
        public int spriteIndex;

        [XmlElement("Shape")]
        public Shape[] shapes;

        public Body Body
        {
            get;
            private set;
        }

        public void Initialize()
        {
            float size = 0f;
            foreach (Shape shape in shapes)
            {
                size = Math.Max(size, shape.GetMaxDistance());
                if (shape.GetType() == typeof(Polygon))
                    ((Polygon)shape).CalculateNormals();
            }
            size *= 2f;

            Body = new Body(shapes, size, Vector2.Zero, Vector2.Zero, PhysicalType.Projectile, null, false);
        }
    }
}