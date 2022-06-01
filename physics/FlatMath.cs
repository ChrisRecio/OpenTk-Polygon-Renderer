using System;

namespace FlatPhysics
{
    public static class FlatMath
    {

        // Magnitude
        public static float Length(FlatVector obj)
        {
            // Sqrt(x^2 + y^2)
            return MathF.Sqrt((obj.X * obj.X) + (obj.Y * obj.Y));
        }

        // Distance of 2 vectors
        public static float Distance(FlatVector a, FlatVector b)
        {
            // Sqrt((a.x - b.x)^2 + (a.y - b.y)^2)
            float i = a.X - b.X;
            float j = a.Y - b.Y;
            return MathF.Sqrt(i * i + j * j);
        }

        // Normalize a vector
        public static FlatVector Normalize(FlatVector a)
        {
            // (Original FlatVector) / |FlatVector|
            // |FlatVector| == Magnitude
            float magnitude = FlatMath.Length(a);
            return new FlatVector((a.X / magnitude), (a.Y / magnitude));
        }

        // Dot product
        public static float Dot(FlatVector a, FlatVector b)
        {
            // (FlatVector a.X + FlatVector a.Y) + (FlatVector b.X + FlatVector b.Y)
            return (a.X * a.Y) + (b.X * b.Y);
        }

        // Cross product
        public static float Cross(FlatVector a, FlatVector b)
        {
            // Cx = AyBz − AzBy
            // Cy = AzBx − AxBz
            // Cz = AxBy − AyBx

            // Since 2d vector, z = 0 so Cx and Cy == 0

            return (a.X * b.Y) - (a.Y * b.X);
        }

    }
}
