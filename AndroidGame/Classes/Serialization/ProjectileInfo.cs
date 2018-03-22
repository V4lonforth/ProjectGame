using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using AndroidGame.Physics;

namespace AndroidGame.Serialization
{
    [XmlRoot("Projectile")]
    public class ProjectileInfo : ISerializationInfo
    {
        [XmlElement("Speed")]
        public float speed;

        [XmlElement("SpriteSize")]
        public Vector2 spriteSize;

        [XmlElement("SpriteIndex")]
        public int spriteIndex;

        [XmlElement("ProjectileType")]
        public int projectileType;

        [XmlElement("Body")]
        public BodyInfo bodyInfo;

        public void Initialize()
        {
            bodyInfo.Initialize(PhysicalType.Projectile);
        }
    }
}