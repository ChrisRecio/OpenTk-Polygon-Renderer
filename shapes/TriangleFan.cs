using OpenTK.Mathematics;

namespace Collision_Simulation.shapes
{
    public class TriangleFan
    {
        private int _nTriangles, _radius;
        private Vector2 _position;
        private float _velocity, _mass;
        private Color4 _color;

        public TriangleFan(int nTriangles, int radius, Vector2 position, float velocity, float mass, Color4 color)
        {
            this._nTriangles = nTriangles;
            this._radius = radius;
            this._position = position;
            this._velocity = velocity;
            this._mass = mass;
            this._color = color;
        }

        // Getters and Setter
        public int NTriangles { get => _nTriangles; set => _nTriangles = value; }
        public int Radius { get => _radius; set => _radius = value; }
        public Vector2 Position { get => _position; set => _position = value; }
        public float Velocity { get => _velocity; set => _velocity = value; }
        public float Mass { get => _mass; set => _mass = value; }
        public Color4 Color { get => _color; set => _color = value; }

        // shaderProgram.setUniformMatrix("transformationMatrix", CreateTransformationMatrix(Vector3 position, Vector3 eulerAngles, Vector3 scale))
        public static Matrix4 CreateTransformationMatrix(Vector3 position, Vector3 eulerAngles, Vector3 scale)
        {
            Matrix4 Translation = Matrix4.CreateTranslation(position.X, position.Y, position.Z);
            Matrix4 Rotation = Matrix4.CreateRotationX(eulerAngles.X) * Matrix4.CreateRotationY(eulerAngles.Y) * Matrix4.CreateRotationZ(eulerAngles.Z);
            Matrix4 Scale = Matrix4.CreateScale(scale);

            return Translation * Rotation * Scale;
        }

        public override bool Equals(object? obj)
        {
            return obj is TriangleFan fan &&
                   NTriangles == fan.NTriangles &&
                   _radius == fan._radius &&
                   _position.Equals(fan._position) &&
                   _velocity == fan._velocity &&
                   _mass == fan._mass;
        }
    }
}