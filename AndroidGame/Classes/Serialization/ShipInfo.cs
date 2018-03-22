using System;
using System.Xml.Serialization;
using AndroidGame.Physics;

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

        [XmlElement("Body")]
        public BodyInfo bodyInfo;

        public void Initialize()
        {
            bodyInfo.Initialize(PhysicalType.Ship);
            rotationSpeed = rotationSpeed / 180f * (float)Math.PI;
        }
    }
}