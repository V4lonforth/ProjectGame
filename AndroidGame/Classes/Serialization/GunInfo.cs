using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace AndroidGame.Serialization
{
    public class GunInfo
    {
        [XmlElement("Rate")]
        public float shootRate;
        [XmlElement("ShotsType")]
        public int shotsType;

        [XmlElement("ShootPosition")]
        public Vector2[] shootPositions;
    }
}