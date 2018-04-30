using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace AndroidGame.Serialization
{
    public class GunInfo
    {
        [XmlElement("ProjectileSpeed")]
        public float projectileSpeed;

        [XmlElement("Damage")]
        public float damage;

        [XmlElement("ShootRate")]
        public float shootRate;

        [XmlElement("ProjectilesType")]
        public int projectilesType;

        [XmlElement("ShootPosition")]
        public Vector2[] shootPositions;
    }
}