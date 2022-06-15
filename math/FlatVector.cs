
namespace FlatPhysics
{
    public readonly struct FlatVector
    {
        public readonly float X;
        public readonly float Y;

        public FlatVector(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        // Addition
        public static FlatVector operator +(FlatVector a, FlatVector b)
        {
            return new FlatVector(a.X + b.X, a.Y + b.Y);
        }

        // Subtraction
        public static FlatVector operator -(FlatVector a, FlatVector b)
        { 
            return new FlatVector(a.X - b.X, a.Y - b.Y);
        }

        // Multiply by a scalar
        public static FlatVector operator *(FlatVector a, float s)
        {
            return new FlatVector(a.X * s, a.Y * s);
        }

        // Divide by a scalar
        public static FlatVector operator /(FlatVector a, float s)
        {
            return new FlatVector(a.X / s, a.Y / s);
        }

        // Flip sign
        public static FlatVector operator -(FlatVector a)
        { 
            return new FlatVector(-a.X, -a.Y);
        }

        // Equals
        public bool Equal(FlatVector a)
        {
            return this.X == a.X && this.Y == a.Y;
        }

        // To string
        public override string ToString()
        {
            return $"{this.X}, {this.X}";
        }
    }
}
