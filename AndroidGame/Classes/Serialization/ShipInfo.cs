using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using AndroidGame.Physics;
using AndroidGame.Geometry;

namespace AndroidGame.Serialization
{
    [XmlRoot("Ship")]
    public class ShipInfo : ISerializationInfo
    {
        [XmlElement("RotationSpeed")]
        public float rotationSpeed;

        [XmlElement("MaxSpeed")]
        public float maxSpeed;

        [XmlElement("Acceleration")]
        public float acceleration;

        [XmlElement("Health")]
        public float health;

        [XmlElement("ShipType")]
        public int shipType;

        [XmlElement("LevelUpExperience")]
        public float levelUpExperience;

        [XmlElement("LevelUpShipType")]
        public int[] levelUpShipTypes;

        [XmlElement("Gun")]
        public GunInfo gunInfo;

        [XmlElement("ParticleSpawner")]
        public ParticleSpawnerInfo spawnerInfo;

        [XmlElement("Body")]
        public BodyInfo bodyInfo;

        [XmlElement("SpriteSize")]
        public Vector2 spriteSize;

        [XmlElement("SizeMultiplier")]
        public float sizeMultiplier;

        [XmlElement("Color")]
        public Color color;

        public ShipInfo()
        {
            color = Color.White;
            sizeMultiplier = 1f;
        }

        public void Initialize()
        {
            rotationSpeed = Functions.DegreeToRadians(rotationSpeed);

            spriteSize *= sizeMultiplier;
            bodyInfo.ChangeSize(sizeMultiplier);
            spawnerInfo.ChangeSize(sizeMultiplier);

            spawnerInfo.Initialize();
            bodyInfo.Initialize(PhysicalType.Ship);
        }
    }
}