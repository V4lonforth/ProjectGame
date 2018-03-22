using System;
using Microsoft.Xna.Framework;
using AndroidGame.Geometry;
using AndroidGame.GameObjects.Base;
using AndroidGame.Serialization;

namespace AndroidGame.Physics
{
    public class Body
    {
        private Rectangle AABB;
        private Shape[] baseShapes;

        private Shape[] realShapes;
        
        private Vector2 position;

        public Vector2 Direction { get; set; }
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                AABB.X = (int)position.X - AABB.Width / 2;
                AABB.Y = (int)position.Y - AABB.Height / 2;
            }
        }
        
        public PhysicalObject Parent
        {
            get;
            private set;
        }

        public Func<Body, bool> OnCollisionAction { get; private set; }

        private static PhysicsController physicsController = new PhysicsController();

        public Body(BodyInfo bodyInfo, Func<Body, bool> onCollision, Vector2 pos, Vector2 dir, PhysicalObject par, bool includeInPhysics = true)
        {
            Parent = par;
            OnCollisionAction = onCollision;
            
            baseShapes = bodyInfo.shapes;
            realShapes = new Shape[bodyInfo.shapes.Length];
            for (int i = 0; i < realShapes.Length; i++)
                realShapes[i] = (Shape)bodyInfo.shapes[i].Clone();

            AABB.Height = AABB.Width = (int)bodyInfo.Size;
            Position = pos;
            Direction = dir;

            if (includeInPhysics)
                physicsController.AddBody(this, bodyInfo.PhysicalType);
        }

        public static void SetPhysicsController(PhysicsController controller)
        {
            physicsController = controller;
        }

        public void Move(Vector2 offset)
        {
            Position += offset;
        }
        public void Rotate(float angle)
        {
            Direction += Functions.RotateVector2(Direction, angle);
        }

        public bool CheckCollision(Body body, CollisionChecker checker)
        {
            if (!CheckAABBCollision(ref body.AABB))
                return false;
            RefreshState();
            body.RefreshState();
            foreach (Shape shape in realShapes)
                foreach (Shape bodyShape in body.realShapes)
                    if (checker.CheckCollision(shape, bodyShape))
                        return true;
            return false;
        }
        private bool CheckAABBCollision(ref Rectangle bodyAABB)
        {
            if (AABB.Right < bodyAABB.Left || AABB.Left > bodyAABB.Right)
                return false;
            if (AABB.Bottom < bodyAABB.Top || AABB.Top > bodyAABB.Bottom)
                return false;
            return true;
        }

        private void RefreshState()
        {
            for (int i = 0; i < baseShapes.Length; i++)
            {
                realShapes[i].Rotate(baseShapes[i], Direction);
                realShapes[i].Move(baseShapes[i], Position);
            }
        }
    }
}