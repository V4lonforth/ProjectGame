using System;
using System.Xml.Serialization;
using AndroidGame.Geometry;
using AndroidGame.Physics;

namespace AndroidGame.Serialization
{
    public class BodyInfo
    {
        [XmlElement("Shape")]
        public Shape[] shapes;

        public PhysicalType PhysicalType { get; private set; }

        public float Size { get; private set; }

        public void Initialize(PhysicalType physicalType)
        {
            Size = 0f;
            foreach (Shape shape in shapes)
            {
                Size = Math.Max(Size, shape.GetMaxDistance());
                if (typeof(Polygon) == shape.GetType())
                    ((Polygon)shape).CalculateNormals();
            }
            PhysicalType = physicalType;
            Size *= 2f;
        }
    }
}