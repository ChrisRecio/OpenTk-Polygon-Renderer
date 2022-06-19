using OpenTK.Mathematics;

namespace Collision_Simulation.shapes
{
    public class TriangleFan
    {
        private int _nTriangles, _radius;
        private Vector3 _position, _eulerAngles, _scale;
        private float _velocity, _mass;
        private Color4 _color;

        public TriangleFan(int nTriangles, int radius, Vector3 position, Vector3 eulerAngles, Vector3 scale, float velocity, float mass, Color4 color)
        {
            this._nTriangles = nTriangles;
            this._radius = radius;
            this._position = position;
            this._eulerAngles = eulerAngles;
            this._scale = scale;
            this._velocity = velocity;
            this._mass = mass;
            this._color = color;
        }

        // Getters and Setter
        public int NTriangles { get => _nTriangles; set => _nTriangles = value; }
        public int Radius { get => _radius; set => _radius = value; }
        public Vector3 Position { get => _position; set => _position = value; }
        public Vector3 EulerAngles { get => _eulerAngles; set => _eulerAngles = value; }
        public Vector3 Scale { get => _scale; set => _scale = value; }
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

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(_nTriangles);
            hash.Add(_radius);
            hash.Add(_position);
            hash.Add(_velocity);
            hash.Add(_mass);
            hash.Add(_color);
            hash.Add(NTriangles);
            hash.Add(Radius);
            hash.Add(Position);
            hash.Add(Velocity);
            hash.Add(Mass);
            hash.Add(Color);
            return hash.ToHashCode();
        }
    }
}