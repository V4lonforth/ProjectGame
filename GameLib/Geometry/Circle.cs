using Microsoft.Xna.Framework;

namespace GameLib.Geometry
{
    public class Circle : Shape
    {
        public Vector2 centre;
        public float radius;
        
        public Circle() { }
        public Circle(Vector2 cent, float rad)
        {
            centre = cent;
            radius = rad;
        }

        public override object Clone()
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

        public override void ChangeSize(float multiplier)
        {
            centre *= multiplier;
            radius *= multiplier;
        }
    }
}
