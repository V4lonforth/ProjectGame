using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using AndroidGame.Physics;
using AndroidGame.Geometry;

namespace AndroidGame.Serialization
{
    [XmlRoot("Projectile")]
    public class ProjectileInfo : ISerializationInfo
    {
        [XmlElement("RotationSpeed")]
        public float rotationSpeed;

        [XmlElement("SpriteSize")]
        public Vector2 spriteSize;
        
        [XmlElement("ProjectileType")]
        public int projectileType;

        [XmlElement("Body")]
        public BodyInfo bodyInfo;

        [XmlElement("ParticleSpawner")]
        public ParticleSpawnerInfo spawnerInfo;

        [XmlElement("SizeMultiplier")]
        public float sizeMultiplier;

        public void Initialize()
        {
            bodyInfo.ChangeSize(sizeMultiplier);
            spawnerInfo.ChangeSize(sizeMultiplier);
            spriteSize *= sizeMultiplier;
            rotationSpeed = Functions.DegreeToRadians(rotationSpeed);

            spawnerInfo.Initialize();
            bodyInfo.Initialize(PhysicalType.Projectile);
        }
    }
}