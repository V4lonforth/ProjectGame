using System;
using Microsoft.Xna.Framework;
using GameLib.Geometry;

namespace GameLib.Physics
{
    public class CollisionChecker
    {
        public bool CheckCollision(Shape shape1, Shape shape2)
        {
            if (shape1.GetType() == typeof(Polygon))
            {
                if (shape2.GetType() == typeof(Polygon))
                    return CheckCollision((Polygon)shape1, (Polygon)shape2);
                else
                    return CheckCollision((Circle)shape2, (Polygon)shape1);
            }
            else
            {
                if (shape2.GetType() == typeof(Polygon))
                    return CheckCollision((Circle)shape1, (Polygon)shape2);
                else
                    return CheckCollision((Circle)shape1, (Circle)shape2);
            }
        }

        protected bool CheckCollision(Circle circle1, Circle circle2)
        {
            Vector2 diff = circle1.centre - circle2.centre;
            float sum = circle1.radius + circle2.radius;
            return diff.X * diff.X + diff.Y * diff.Y <= sum * sum;
        }

        protected bool CheckCollision(Circle circle, Polygon polygon)
        {
            return GetMinDistance(polygon, circle) <= circle.radius;
        }
        protected float GetMinDistance(Polygon polygon, Circle circle)
        {
            float minDistance = float.MaxValue;
            for (int i = 0; i < polygon.vertices.Length; i++)
                minDistance = System.Math.Min(minDistance, GetDistance(circle.centre, polygon, i));
            return minDistance;
        }
        private float GetDistance(Vector2 point, Polygon polygon, int ind)
        {
            if (Functions.DotProduct(point, polygon.vertices[ind]) <= 0f)
                return (point - polygon.vertices[ind]).Length();
            if (Functions.DotProduct(point, polygon.vertices[(ind + 1) % polygon.vertices.Length]) <= 0f)
                return (point - polygon.vertices[(ind + 1) % polygon.vertices.Length]).Length();
            return System.Math.Abs(Functions.DotProduct(point - polygon.vertices[ind], polygon.normals[ind]));
        }

        protected bool CheckCollision(Polygon polygon1, Polygon polygon2)
        {
            return GetDepth(polygon1, polygon2) <= 0f && GetDepth(polygon2, polygon1) <= 0f;
        }
        private float GetDepth(Polygon polygon1, Polygon polygon2)
        {
            float maxDistance = float.MinValue;
            for (int i = 0; i < polygon1.vertices.Length; i++)
            {
                int supportPointIndex = GetSupportPointIndex(polygon2, -polygon1.normals[i]);
                maxDistance = Math.Max(maxDistance, Functions.DotProduct(polygon1.normals[i], polygon2.vertices[supportPointIndex] - polygon1.vertices[i]));
            }
            return maxDistance;
        }
        private int GetSupportPointIndex(Polygon polygon, Vector2 direction)
        {
            int maxIndex = 0;
            float maxValue = float.MinValue;
            for (int i = 0; i < polygon.vertices.Length; i++)
            {
                float dot = Functions.DotProduct(polygon.vertices[i], direction);
                if (dot > maxValue)
                {
                    maxValue = dot;
                    maxIndex = i;
                }
            }
            return maxIndex;
        }
    }
}