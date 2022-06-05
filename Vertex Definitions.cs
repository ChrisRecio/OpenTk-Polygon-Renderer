using OpenTK.Mathematics;

namespace Collision_Simulation
{

    public readonly struct VertexAttribute
    {
        public readonly string Name;
        public readonly int Index;
        public readonly int ComponentCount;
        public readonly int Offset;

        public VertexAttribute(string Name, int Index, int ComponentCount, int Offset)
        {
            this.Name = Name;
            this.Index = Index;
            this.ComponentCount = ComponentCount;
            this.Offset = Offset;
        }
    }


    public sealed class VertexInfo
    {
        public readonly Type? Type;
        public readonly int SizeInBytes;
        public readonly VertexAttribute[]? VertexAttributes;

        public VertexInfo(Type Type, params VertexAttribute[] attributes)
        {
            this.Type = Type;
            this.SizeInBytes = 0;
            this.VertexAttributes = attributes;

            for(int i = 0; i < this.VertexAttributes.Length; i++)
            {
                VertexAttribute attribute = this.VertexAttributes[i];
                this.SizeInBytes += attribute.ComponentCount * sizeof(float);
            }
        }
    }


    public readonly struct VertexPositionColor
    {
        public readonly Vector2 Position;
        public readonly Color4 Color;
        public static readonly VertexInfo VertexInfo = new VertexInfo(
            typeof(VertexPositionColor),
            new VertexAttribute("Position", 0, 2, 0),
            new VertexAttribute("Color", 1, 4, 2 * sizeof(float))
            );

        public VertexPositionColor(Vector2 Position, Color4 Color)
        {
            this.Position = Position;
            this.Color = Color;
        }
    }
}

