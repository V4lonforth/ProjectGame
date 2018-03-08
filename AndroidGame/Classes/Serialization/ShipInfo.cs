using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using AndroidGame.Physics;
using AndroidGame.Geometry;

namespace AndroidGame.Serialization
{
    [XmlRoot("Ship")]
    public class ShipInfo : ISerializationInfo
    {
        [XmlElement("MaxSpeed")]
        public float maxSpeed;

        [XmlElement("Acceleration")]
        public float acceleration;

        [XmlElement("Health")]
        public float health;

        [XmlElement("Experience")]
        public float experience;

        [XmlElement("Type")]
        public int type;

        [XmlElement("NextShipType")]
        public int[] nextShipsTypes;

        [XmlElement("ShipPart")]
        public ShipPartInfo[] shipParts;

        [XmlElement("Shape")]
        public Shape[] shapes;

        [XmlElement("Gun")]
        public GunInfo gun;

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
                if (typeof(Polygon) == shape.GetType())
                    ((Polygon)shape).CalculateNormals();
            }
            size *= 2f;
            Body = new Body(shapes, size, Vector2.Zero, Vector2.Zero, PhysicalType.Ship, null, false);
        }
    }
}