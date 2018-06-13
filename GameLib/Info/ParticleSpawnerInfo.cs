using Microsoft.Xna.Framework;

namespace GameLib.Info
{
    public class ParticleSpawnerInfo
    {
        public Vector2[] positions;

        public float rotationSpeed;
        public float speed;
        public float deceleration;
        public float lifeTime;
        
        public float spawnRate;
        public float radius;
        public Color color;

        public void ChangeSize(float multiplier)
        {
            for (int i = 0; i < positions.Length; i++)
                positions[i] *= multiplier;
        }
    }
}