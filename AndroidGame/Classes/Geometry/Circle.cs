using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace AndroidGame.Geometry
{
    public class Circle : Shape
    {
        [XmlElement("Centre")]
        public Vector2 centre;
        [XmlElement("Radius")]
        public float radius;
        
        public Circle() { }
        public Circle(Vector2 cent, float rad)
        {
            centre = cent;
            radius = rad;
        }

        public override Shape Copy()
        {
            return new Circle(centre, radius);
        }
        public override void Rotate(Shape baseShape, Vector2 dir)
        {
            centre = Functions.RotateVector2(((Circle)baseShape).centre, dir);
        }
        public override void Move(Shape baseShape, Vector2 offset)
        {
            centre += offset;
        }
        public override float GetMaxDistance()
        {
            return centre.Length() + radius;
        }

        public override bool CheckCollision(Shape shape)
        {
            if (shape.GetType() == typeof(Circle))
                return CheckCollision(this, (Circle)shape);
            return CheckCollision(this, (Polygon)shape);
        }
    }
}
