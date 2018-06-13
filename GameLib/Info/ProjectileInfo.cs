using Microsoft.Xna.Framework;
using GameLib.Physics;
using GameLib.Geometry;
using Microsoft.Xna.Framework.Graphics;

namespace GameLib.Info
{
    public class ProjectileInfo
    {
        public Texture2D texture;

        public float rotationSpeed;

        public Vector2 spriteSize;
        public int projectileType;

        public BodyInfo bodyInfo;
        public ParticleSpawnerInfo spawnerInfo;

        public float sizeMultiplier;
        public Color color;
        
        public static ProjectileInfo[] GetProjectilesInfo(Texture2D[] textures)
        {
            ProjectileInfo[] projectilesInfo = new ProjectileInfo[]
            {
                new ProjectileInfo()
                {
                    texture = textures[0],
                    rotationSpeed = Functions.DegreeToRadians(135f),
                    projectileType = 0,
                    spriteSize = new Vector2(32f, 32f),
                    sizeMultiplier = 1f,
                    color = Color.White,
                    bodyInfo = new BodyInfo(PhysicalType.Projectile, new Shape[]
                    {
                        new Polygon(new Vector2[]
                        {
                            new Vector2(-28f, 32f),
                            new Vector2(30f, 0f),
                            new Vector2(-28f, 32f)
                        })
                    }),
                    spawnerInfo = new ParticleSpawnerInfo()
                    {
                        positions = new Vector2[]
                        {
                            new Vector2(-12.5f, -13.5f),
                            new Vector2(-12.5f, 13.5f),
                            new Vector2(12f, 0f)
                        },
                        rotationSpeed = Functions.DegreeToRadians(270f),
                        speed = 0.5f,
                        deceleration = 0.5f,
                        lifeTime = 1.2f,
                        spawnRate = 35f,
                        radius = 6f,
                        color = Color.White
                    }
                }
            };
            foreach (ProjectileInfo info in projectilesInfo)
            {
                info.bodyInfo.ChangeSize(info.sizeMultiplier);
                info.spawnerInfo.ChangeSize(info.sizeMultiplier);
                info.spriteSize *= info.sizeMultiplier;
            }
            return projectilesInfo;
        }
    }
}