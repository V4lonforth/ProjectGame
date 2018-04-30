using System;
using Microsoft.Xna.Framework;

namespace AndroidGame.Geometry
{
    public static class Functions
    {
        public static Random random = new Random();

        private const float PI = (float)Math.PI;
        private const float PI2 = PI * 2;

        public static Vector2 RandomVector2()
        {
            float angle = (float)(random.NextDouble()) * PI2;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
        public static Vector2 RandomVector2(float radius)
        {
            return RandomVector2() * radius;
        }
        public static Vector2 RandomVector2(float minRadius, float maxRadius)
        {
            return RandomVector2() * ((float)(random.NextDouble() * (maxRadius - minRadius)) + minRadius);
        }

        public static Vector2 RotateVector2(Vector2 vec, float angle)
        {
            Vector2 vector;
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            vector.X = vec.X * cos - vec.Y * sin;
            vector.Y = vec.Y * cos + vec.X * sin;
            return vector;
        }
        public static Vector2 RotateVector2(Vector2 vec1, Vector2 vec2)
        {
            Vector2 vector;
            vector.X = vec1.X * vec2.X - vec1.Y * vec2.Y;
            vector.Y = vec1.Y * vec2.X + vec1.X * vec2.Y;
            return vector;
        }

        public static float Interpolate(float a, float b, float t)
        {
            return (b - a) * t + a;
        }
        public static Vector2 Interpolate(Vector2 a, Vector2 b, float t)
        {
            a.X = Interpolate(a.X, b.X, t);
            a.Y = Interpolate(a.Y, b.Y, t);
            return a;
        }

        public static float CircleLerp(float a, float b, float speed)
        {
            if (a > b)
            {
                if (a - b > PI)
                {
                    a += speed;
                    if (a > PI)
                    {
                        a -= PI2;
                        if (a > b)
                            a = b;
                    }
                }
                else
                {
                    a -= speed;
                    if (a < b)
                        a = b;
                    else if (a < -PI)
                        a += PI2;
                }
            }
            else
            {
                if (b - a > PI)
                {
                    a -= speed;
                    if (a < -PI)
                    {
                        a += PI2;
                        if (a < b)
                            a = b;
                    }
                }
                else
                {
                    a += speed;
                    if (a > b)
                        a = b;
                    else if (a > PI)
                        a -= PI2;
                }
            }
            return a;
        }
        public static Vector2 CircleLerp(Vector2 a, Vector2 b, float speed)
        {
            float angle = CircleLerp((float)Math.Atan2(a.Y, a.X), (float)Math.Atan2(b.Y, b.X), speed);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static Vector2 CircleInterpolate(Vector2 a, Vector2 b, float t)
        {
            float firstAngle = (float)Math.Atan2(a.Y, a.X);
            float secondAngle = (float)Math.Atan2(b.Y, b.X);
            float angle = CircleLerp(firstAngle, secondAngle, t * Math.Abs(firstAngle - secondAngle));
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static float DotProduct(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }
        public static float CrossProduct(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }

        public static float DegreeToRadians(float degree)
        {
            return degree / 180f * (float)Math.PI;
        }
    }
}