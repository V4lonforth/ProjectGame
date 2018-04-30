using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AndroidGame.Geometry;
using AndroidGame.GUI;

namespace AndroidGame.Controllers
{
    public class ParticleSystem
    {
        struct VertexData : IVertexType
        {
            public Vector2 position;
            public Vector2 localPosition;
            public Vector2 direction;

            public float speed;
            public float deceleration;

            public float rotation;

            public float startTime;
            public float endTime;

            public Color color;

            public VertexData(Vector2 pos, Vector2 localPos, Vector2 dir, float sp, float decel, float rot, float startT, float endT, Color col)
            {
                localPosition = localPos;
                position = pos;
                direction = dir;
                speed = sp;
                deceleration = decel;
                rotation = rot;
                startTime = startT;
                endTime = endT;
                color = col;
            }

            public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
                new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.Position, 2),
                new VertexElement(sizeof(float) * 6, VertexElementFormat.Single, VertexElementUsage.PointSize, 0),
                new VertexElement(sizeof(float) * 7, VertexElementFormat.Single, VertexElementUsage.PointSize, 1),
                new VertexElement(sizeof(float) * 8, VertexElementFormat.Single, VertexElementUsage.PointSize, 2),
                new VertexElement(sizeof(float) * 9, VertexElementFormat.Single, VertexElementUsage.PointSize, 3),
                new VertexElement(sizeof(float) * 10, VertexElementFormat.Single, VertexElementUsage.PointSize, 4),
                new VertexElement(sizeof(float) * 11, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            );

            VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
        };

        private VertexData[] verticesData;
        private int index;

        private Camera camera;

        private Effect effect;

        private Vector2 triangleRightVertex;
        private Vector2 triangleLeftVertex;

        private float currentTime;

        private GraphicsDevice graphicsDevice;

        private const int particlesCount = 2000;

        private const float triangleAngle = (float)Math.PI / 1.5f;

        public ParticleSystem(ContentManager content, Camera cam, GraphicsDevice graphDevice)
        {
            effect = content.Load<Effect>("Shaders/ParticleShader");
            camera = cam;
            graphicsDevice = graphDevice;

            verticesData = new VertexData[particlesCount * 3];
            index = 0;

            effect.Parameters["width"].SetValue((float)(GUIController.screenSize.X / 2));
            effect.Parameters["height"].SetValue((float)(GUIController.screenSize.Y / 2));

            triangleRightVertex = Functions.RotateVector2(Vector2.UnitX, triangleAngle);
            triangleLeftVertex = Functions.RotateVector2(Vector2.UnitX, -triangleAngle);
        }
        
        public void Update(float deltaTime)
        {
            currentTime += deltaTime;
        }
        public void Draw()
        {
            effect.Parameters["worldMatrix"].SetValue(camera.TransformMatrix);
            effect.Parameters["currentTime"].SetValue(currentTime);
            effect.Techniques[0].Passes[0].Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verticesData, 0, particlesCount);
        }

        private int FindAvailable()
        {
            index = (index + 1) % particlesCount;
            return index * 3;
        }

        public void CreateParticle(Vector2 position, Vector2 lookingDirection, float radius, Vector2 movingDirection, float speed, float deceleration, float rotationSpeed, float lifeTime, Color color)
        {
            int index = FindAvailable();
            verticesData[index] = new VertexData(position, lookingDirection * radius, movingDirection, speed, deceleration, rotationSpeed, currentTime, currentTime + lifeTime, color);
            verticesData[index + 1] = new VertexData(position, Functions.RotateVector2(lookingDirection, triangleRightVertex) * radius, movingDirection, speed, deceleration, rotationSpeed, currentTime, currentTime + lifeTime, color);
            verticesData[index + 2] = new VertexData(position, Functions.RotateVector2(lookingDirection, triangleLeftVertex) * radius, movingDirection, speed, deceleration, rotationSpeed, currentTime, currentTime + lifeTime, color);
        }
    }
}