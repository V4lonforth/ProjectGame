using Microsoft.Xna.Framework;
using System;

namespace GameLib.Geometry
{
    public class Polygon : Shape
    {
        public Vector2[] vertices;
        public Vector2[] normals;

        public Polygon()
        {
        }
        public Polygon(Vector2[] vert)
        {
            vertices = vert;
        }

        public void CalculateNormals()
        {
            normals = new Vector2[vertices.Length];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i].X = vertices[(i + 1) % vertices.Length].Y - vertices[i].Y;
                normals[i].Y = vertices[i].X - vertices[(i + 1) % vertices.Length].X;
                normals[i].Normalize();
            }
        }

        public override object Clone()
        {
            Polygon polygon = new Polygon
            {
                vertices = new Vector2[vertices.Length],
                normals = new Vector2[normals.Length]
            };
            for (int i = 0; i < polygon.vertices.Length; i++)
            {
                polygon.vertices[i] = vertices[i];
                polygon.normals[i] = normals[i];
            }
            return polygon;
        }
        public override void Rotate(Shape baseShape, Vector2 dir)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = Functions.RotateVector2(((Polygon)baseShape).vertices[i], dir);
                normals[i] = Functions.RotateVector2(((Polygon)baseShape).normals[i], dir);
            }
        }
        public override void Move(Shape baseShape, Vector2 offset)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] += offset;
        }
        public override float GetMaxDistance()
        {
            float maxDistance = 0f;
            for (int i = 0; i < vertices.Length; i++)
                maxDistance = Math.Max(maxDistance, vertices[i].Length());
            return maxDistance;
        }

        public override void ChangeSize(float multiplier)
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] *= multiplier;
        }
    }
}
