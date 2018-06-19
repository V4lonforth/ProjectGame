using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLib.Physics;
using GameLib.Geometry;

namespace GameLib.Info
{
    public class ShipInfo
    {
        public Texture2D texture;

        public float rotationSpeed;
        public float maxSpeed;
        public float acceleration;
        
        public float health;
        public int shipType;

        public float levelUpExperience;
        public int[] levelUpShipTypes;
        
        public GunInfo gunInfo;
        public ParticleSpawnerInfo spawnerInfo;
        public BodyInfo bodyInfo;
        
        public Vector2 spriteSize;
        public float sizeMultiplier;
        public Color color;

        public ShipInfo()
        {
            color = Color.White;
            sizeMultiplier = 1f;
        }
        public static ShipInfo[] GetShipsInfo(Texture2D[] textures)
        {
            ShipInfo[] shipsInfo = GetShipsInfo();
            for (int i = 0; i < shipsInfo.Length; i++)
                shipsInfo[i].texture = textures[i];
            return shipsInfo;
        }
        public static ShipInfo[] GetShipsInfo()
        {
            ShipInfo[] shipsInfo = new ShipInfo[]
            {
                new ShipInfo()
                {
                    rotationSpeed = Functions.DegreeToRadians(280f),
                    maxSpeed = 450f,
                    acceleration = 320f,
                    health = 100f,
                    shipType = 0,
                    levelUpExperience = 1000,
                    levelUpShipTypes = new int[]
                    {
                        1, 2, 3
                    },
                    spriteSize = new Vector2(256),
                    sizeMultiplier = 1f,
                    color = Color.White,
                    gunInfo = new GunInfo()
                    {
                        shootPositions = new Vector2[]
                        {
                            new Vector2(60f, 0f)
                        },
                        projectileSpeed = 900f,
                        damage = 20f,
                        shootRate = 2f,
                        projectilesType = 0
                    },
                    bodyInfo = new BodyInfo(PhysicalType.Ship, new Shape[]
                    {
                        new Polygon(new Vector2[]
                        {
                            new Vector2(-82f, -88f),
                            new Vector2(125f, 0f),
                            new Vector2(-82f, 88f)
                        })
                    }),
                    spawnerInfo = new ParticleSpawnerInfo()
                    {
                        positions = new Vector2[]
                        {
                            new Vector2(-98f, -68f),
                            new Vector2(-98f, 68),
                            new Vector2(85f, 0f)
                        },
                        rotationSpeed = 0f,
                        speed = 0f,
                        deceleration = 0f,
                        lifeTime = 1f,
                        spawnRate = 15f,
                        radius = 9f,
                        color = Color.White
                    }
                },

                new ShipInfo()
                {
                    rotationSpeed = Functions.DegreeToRadians(360f),
                    maxSpeed = 500f,
                    acceleration = 350f,
                    health = 90f,
                    shipType = 1,
                    levelUpExperience = 1500,
                    levelUpShipTypes = new int[]
                    {
                    },
                    spriteSize = new Vector2(256),
                    sizeMultiplier = 1f,
                    color = Color.White,
                    gunInfo = new GunInfo()
                    {
                        shootPositions = new Vector2[]
                        {
                            new Vector2(60f, 0f)
                        },
                        projectileSpeed = 1000f,
                        damage = 18f,
                        shootRate = 2.5f,
                        projectilesType = 0
                    },
                    bodyInfo = new BodyInfo(PhysicalType.Ship, new Shape[]
                    {
                        new Polygon(new Vector2[]
                        {
                            new Vector2(-126f, -84f),
                            new Vector2(128f, 0f),
                            new Vector2(-87f, 0f)
                        }),
                        new Polygon(new Vector2[]
                        {
                            new Vector2(128f, 0f),
                            new Vector2(-126f, 84f),
                            new Vector2(-87f, 0f)
                        })
                    }),
                    spawnerInfo = new ParticleSpawnerInfo()
                    {
                        positions = new Vector2[]
                        {
                            new Vector2(-104f, -64f),
                            new Vector2(-104f, 64),
                            new Vector2(90f, 0f)
                        },
                        rotationSpeed = 0f,
                        speed = 0f,
                        deceleration = 0f,
                        lifeTime = 1f,
                        spawnRate = 15f,
                        radius = 9f,
                        color = Color.White
                    }
                }
            };
            foreach (ShipInfo info in shipsInfo)
            {
                info.bodyInfo.ChangeSize(info.sizeMultiplier);
                info.spawnerInfo.ChangeSize(info.sizeMultiplier);
                info.spriteSize *= info.sizeMultiplier;
            }
            return shipsInfo;
        }
        private void ChangeSize(float multiplier)
        {
            bodyInfo.ChangeSize(sizeMultiplier);
            spawnerInfo.ChangeSize(sizeMultiplier);
            spriteSize.X = (int)(spriteSize.X * sizeMultiplier);
            spriteSize.Y = (int)(spriteSize.Y * sizeMultiplier);
        }
    }
}