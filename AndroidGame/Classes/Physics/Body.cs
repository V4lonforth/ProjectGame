using System;
using Microsoft.Xna.Framework;
using AndroidGame.Geometry;

namespace AndroidGame.Physics
{
    public class Body
    {
        private Rectangle AABB;
        private Shape[] baseShapes;

        private Shape[] realShapes;
        
        private Vector2 direction;
        private Vector2 position;

        public Vector2 Direction
        {
            get { return direction; }
            set
            {
                direction = value;
            }
        }
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                AABB.X = (int)position.X - AABB.Width / 2;
                AABB.Y = (int)position.Y - AABB.Height / 2;
            }
        }
        
        private PhysicalType type;
        public object Parent
        {
            get;
            private set;
        }

        public Func<Body, bool> OnCollisionAction
        {
            get;
            set;
        }

        private static PhysicsController physicsController;

        public Body(Shape[] shapes, float size, Vector2 pos, Vector2 dir, PhysicalType type, object par, bool includeInPhysics = true)
        {
            Parent = par;

            this.type = type;
            AABB.Height = AABB.Width = (int)size + 1;

            Position = pos;
            Direction = dir;

            baseShapes = shapes;
            realShapes = new Shape[shapes.Length];

            for (int i = 0; i < shapes.Length; i++)
                realShapes[i] = baseShapes[i].Copy();
            if (includeInPhysics)
                physicsController.AddBody(this, type);
        }
        public Body(Body baseBody, Vector2 pos, Vector2 dir, object par, bool includeInPhysics = true)
        {
            Parent = par;

            baseShapes = baseBody.baseShapes;
            realShapes = new Shape[baseBody.realShapes.Length];
            for (int i = 0; i < realShapes.Length; i++)
                realShapes[i] = baseBody.realShapes[i].Copy();

            AABB = baseBody.AABB;
            Position = pos;

            Direction = dir;
            type = baseBody.type;

            if (includeInPhysics)
                physicsController.AddBody(this, type);
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
            Direction += Functions.RotateVector2(direction, angle);
        }

        public bool CheckCollision(Body body)
        {
            if (!CheckAABBCollision(ref body.AABB))
                return false;
            RefreshState();
            body.RefreshState();
            foreach (Shape shape in realShapes)
                foreach (Shape bodyShape in body.realShapes)
                    if (shape.CheckCollision(bodyShape))
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
                realShapes[i].Rotate(baseShapes[i], direction);
                realShapes[i].Move(baseShapes[i], position);
            }
        }
    }
}