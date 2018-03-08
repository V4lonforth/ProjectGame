using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AndroidGame.Serialization
{
    public class ShipPartInfo
    {
        [XmlElement("Position")]
        public Vector2 localPosition;

        [XmlElement("Rotation")]
        public float localRotation;

        [XmlElement("Color")]
        public Color color;

        [XmlElement("SpriteSize")]
        public Vector2 spriteSize;

        [XmlElement("SpriteIndex")]
        public int spriteIndex;

        [XmlElement("SpriteEffect")]
        public SpriteEffects spriteEffects;

        public ShipPartInfo()
        {
            color = Color.White;
        }
    }
}
