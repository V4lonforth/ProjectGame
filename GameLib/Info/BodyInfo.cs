using System;
using GameLib.Geometry;
using GameLib.Physics;

namespace GameLib.Info
{
    public class BodyInfo
    {
        public Shape[] shapes;
        public PhysicalType physicalType;
        public float size;

        public BodyInfo(PhysicalType physicalType, Shape[] shapes)
        {
            size = 0f;
            this.shapes = shapes;
            foreach (Shape shape in shapes)
            {
                size = Math.Max(size, shape.GetMaxDistance());
                if (typeof(Polygon) == shape.GetType())
                    ((Polygon)shape).CalculateNormals();
            }
            this.physicalType = physicalType;
            size *= 2f;
        }

        public void ChangeSize(float multiplier)
        {
            foreach (Shape shape in shapes)
                shape.ChangeSize(multiplier);
        }
    }
}