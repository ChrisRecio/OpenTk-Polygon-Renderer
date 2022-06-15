using OpenTK.Mathematics;

namespace Collision_Simulation.shapes
{
    public class TrangleFan
    {
        private static Random rand = new Random();
        private double twicePi = 2 * Math.PI;
        private int posX, posY, nTriangles, radius;
        private VertexPositionColor[] vertices;

        public TrangleFan(int posX, int posY, int nTriangles, int radius, VertexPositionColor[] vertices)
        {
            this.posX = posX;
            this.posY = posY;
            this.nTriangles = nTriangles;
            this.radius = radius;
            this.vertices = vertices;
        }

        public Matrix4 CreateTransformationMatrix(Vector3 eulerAngles, Vector3 scale)
        {
            Matrix4 Translation = Matrix4.CreateTranslation(posX, posY, 1.0f);
            Matrix4 Rotation = Matrix4.CreateRotationX(eulerAngles.X) * Matrix4.CreateRotationY(eulerAngles.Y) * Matrix4.CreateRotationZ(eulerAngles.Z);
            Matrix4 Scale = Matrix4.CreateScale(scale);

            return Translation * Rotation * Scale;
        }
    }
}
// WRAP BELOW INTO A CLASS
/*
Random rand = new Random();
double twicePi = 2 * Math.PI;
int windowWidth = this.ClientSize.X;
int windowHeight = this.ClientSize.Y;

int nTriangles = 20; // Triangles per polyogn (n >= 3)
int polygonCount = 10; // How many polygons will be rendered

VertexPositionColor[] vertices = new VertexPositionColor[polygonCount * (nTriangles + 1)]; // polygonCount * nTriangles + 1
this.vertexCount = 0;

for (int i = 0; i < polygonCount; i++)
{
    int radius = rand.Next(25, 50);
    int posX = rand.Next(radius, windowWidth - radius);
    int posY = rand.Next(radius, windowHeight - radius);

    float r = (float)rand.NextDouble();
    float g = (float)rand.NextDouble();
    float b = (float)rand.NextDouble();

    for (int j = 0; j < (nTriangles + 1); j++)
    {
        float x = (float)(posX + (radius * Math.Cos(j * twicePi / nTriangles)));
        float y = (float)(posY + (radius * Math.Sin(j * twicePi / nTriangles)));

        vertices[this.vertexCount++] = new VertexPositionColor(new Vector2(x, y), new Color4(r, g, b, 1.0f));
    }
}

int[] indices = new int[polygonCount * (nTriangles * 3)];
this.indexCount = 0;
this.vertexCount = 0;

for (int i = 0; i < polygonCount; i++)
{

    for (int x = 0; x < nTriangles; x++)
    {
        if (x == nTriangles - 1)
        {
            indices[this.indexCount++] = 0 + this.vertexCount;
            indices[this.indexCount++] = x + 1 + this.vertexCount;
            indices[this.indexCount++] = 1 + this.vertexCount;
        }
        else
        {
            indices[this.indexCount++] = 0 + this.vertexCount;
            indices[this.indexCount++] = x + 1 + this.vertexCount;
            indices[this.indexCount++] = x + 2 + this.vertexCount;
        }

    }
    this.vertexCount += nTriangles + 1;
}
*/