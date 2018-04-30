using System.Xml.Serialization;
using AndroidGame.Geometry;
using Microsoft.Xna.Framework;

namespace AndroidGame.Serialization
{
    public class ParticleSpawnerInfo : ISerializationInfo
    {
        [XmlElement("SpawnPosition")]
        public Vector2[] positions;

        [XmlElement("RotationSpeed")]
        public float rotationSpeed;

        [XmlElement("Speed")]
        public float speed;

        [XmlElement("Deceleration")]
        public float deceleration;

        [XmlElement("LifeTime")]
        public float lifeTime;

        [XmlElement("SpawnRate")]
        public float spawnRate;

        [XmlElement("Radius")]
        public float radius;

        [XmlElement("Color")]
        public Color color;

        public ParticleSpawnerInfo()
        {
            color = Color.White;
        }

        public void ChangeSize(float multiplier)
        {
            for (int i = 0; i < positions.Length; i++)
                positions[i] *= multiplier;
        }

        public void Initialize()
        {
            rotationSpeed = Functions.DegreeToRadians(rotationSpeed);
        }
    }
}