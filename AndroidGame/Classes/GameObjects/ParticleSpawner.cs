using Microsoft.Xna.Framework;
using AndroidGame.Controllers;
using GameLib.Geometry;
using GameLib.Info;
using GameLib.GameObjects.Base;

namespace AndroidGame.GameObjects
{
    class ParticleSpawner
    {
        private ParticleSystem particleSystem;

        private PhysicalObject parent;

        private Vector2[] spawnPositions;

        private float radius;

        private float rotationSpeed;
        private float lifeTime;

        private float speed;
        private float deceleration;

        private float spawnTime;
        private float spawnTimeLeft;

        private Vector2 prevDirection;

        private bool isEnabled;

        private Color color;

        private const float minSpeedToSpawn = 30;

        public ParticleSpawner(ParticleSystem system, PhysicalObject par, ParticleSpawnerInfo info, bool enabled = true)
        {
            particleSystem = system;
            parent = par;
            spawnPositions = info.positions;
            rotationSpeed = info.rotationSpeed;
            lifeTime = info.lifeTime;
            spawnTime = 1f / info.spawnRate;
            radius = info.radius;
            speed = info.speed;
            deceleration = info.deceleration;
            color = info.color;
            isEnabled = enabled;
        }

        public void Update(float deltaTime)
        {
            if (isEnabled)
            {
                if (spawnTimeLeft > 0f)
                    spawnTimeLeft -= deltaTime;
                else if (parent.Speed > minSpeedToSpawn || prevDirection != parent.LookingDirection)
                {
                    float particleSpeed = speed * parent.Speed;
                    float particleDeceleration = deceleration * particleSpeed;
                    spawnTimeLeft = spawnTime;
                    for (int i = 0; i < spawnPositions.Length; i++)
                        particleSystem.CreateParticle(parent.Position + Functions.RotateVector2(spawnPositions[i], parent.LookingDirection), parent.LookingDirection, radius, parent.MovementDirection, particleSpeed, particleDeceleration, rotationSpeed, lifeTime, color);
                }
                prevDirection = parent.LookingDirection;
            }
        }
    }
}