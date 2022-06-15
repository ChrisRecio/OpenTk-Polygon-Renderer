using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Collision_Simulation
{
    internal class Game : GameWindow
    {
        private VertexBuffer vertexBuffer = default!;
        private IndexBuffer indexBuffer = default!;
        private VertexArray vertexArray = default!;
        private ShaderProgram shaderProgram = default!;

        private readonly string vertexShaderLocation = "../../../assets/vertexShader.glsl";
        private readonly string fragmentShaderLocation = "../../../assets/fragmentShader.glsl";

        private int vertexCount, indexCount;
        private float colorFactor = 1f, deltaColorFactor = 1f/256f;

        private readonly Color4 backgroundColor = new Color4(0.5f, 0.5f, 0.5f, 1.0f);
        private readonly Color4 orange = new Color4(1.0f, 0.5f, 0.31f, 1.0f);
        
        public Game(int width = 1280, int height = 768, String title = "Collision Simmulation") : base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                WindowBorder = WindowBorder.Fixed,
                StartVisible = false,
                StartFocused = true,
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                APIVersion = new Version(3, 3)

            })
        {
            this.CenterWindow();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            this.IsVisible = true;
            GL.ClearColor(backgroundColor); // Set Background Color

            Random rand = new Random();
            double twicePi = 2 * Math.PI;
            int windowWidth = this.ClientSize.X;
            int windowHeight = this.ClientSize.Y;

            int nTriangles = 20; // Triangles per polyogn (n >= 3)
            int polygonCount = 10; // How many polygons will be rendered

            VertexPositionColor[] vertices = new VertexPositionColor[polygonCount * (nTriangles + 1)]; // polygonCount * nTriangles + 1
            this.vertexCount = 0;

            for(int i = 0; i < polygonCount; i++)
            {
                int radius = rand.Next(25, 50);
                int posX = rand.Next(radius, windowWidth - radius);
                int posY = rand.Next(radius, windowHeight - radius);

                float r = (float)rand.NextDouble();
                float g = (float)rand.NextDouble();
                float b = (float)rand.NextDouble();

                for(int j = 0; j < (nTriangles + 1); j++)
                {
                    float x = (float)(posX + (radius * Math.Cos(j * twicePi / nTriangles)));
                    float y = (float)(posY + (radius * Math.Sin(j * twicePi / nTriangles)));

                    vertices[this.vertexCount++] = new VertexPositionColor(new Vector2(x, y), new Color4(r, g, b, 1.0f));
                }
            }

            int[] indices = new int[polygonCount * (nTriangles * 3 )];
            this.indexCount = 0;
            this.vertexCount = 0;

            for (int i = 0; i < polygonCount; i++)
            {

                for(int x = 0; x < nTriangles ; x++)
                {
                    if(x == nTriangles - 1)
                    {
                        indices[this.indexCount++] = 0 + this.vertexCount;
                        indices[this.indexCount++] = x+1 + this.vertexCount;
                        indices[this.indexCount++] = 1 + this.vertexCount;
                    }
                    else
                    {
                        indices[this.indexCount++] = 0 + this.vertexCount;
                        indices[this.indexCount++] = x+1 + this.vertexCount;
                        indices[this.indexCount++] = x+2 + this.vertexCount;
                    }
                    
                }
                this.vertexCount += nTriangles + 1;
            }

            this.vertexBuffer = new VertexBuffer(VertexPositionColor.VertexInfo, vertices.Length, true);
            this.vertexBuffer.SetData(vertices, vertices.Length);

            this.indexBuffer = new IndexBuffer(indices.Length, true);
            this.indexBuffer.SetData(indices, indices.Length);

            this.vertexArray = new VertexArray(this.vertexBuffer);

            this.shaderProgram = new ShaderProgram(vertexShaderLocation, fragmentShaderLocation);

            int[] viewport = new int[4]; // x, y, Width, Height
            GL.GetInteger(GetPName.Viewport, viewport);

            this.shaderProgram.setUniform("viewportSize", (float)viewport[2], (float)viewport[3]);
            this.shaderProgram.setUniform("colorFactor", this.colorFactor);

            // ShaderProgram Error Log
            string shaderProgramInfo = GL.GetShaderInfoLog(shaderProgram.ShaderProgramHandle);
            if (shaderProgramInfo != String.Empty)
            {
                Console.WriteLine("shaderProgramHandle Info");
                Console.WriteLine(shaderProgramInfo);
            }

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            this.vertexBuffer?.Dispose();
            this.indexBuffer?.Dispose();
            this.vertexArray?.Dispose();
            this.shaderProgram?.Dispose();

            base.OnUnload();
        }

        // Called per frame update
        protected override void OnUpdateFrame(FrameEventArgs args)
        {  
            this.colorFactor += this.deltaColorFactor;

            if(this.colorFactor >= 1f)
            {
                this.colorFactor = 1f;
                this.deltaColorFactor *= -1f;
            }

            if (this.colorFactor <= 0f)
            {
                this.colorFactor = 0f;
                this.deltaColorFactor *= -1f;
            }

            this.shaderProgram.setUniform("colorFactor", colorFactor);
            // TODO
            // loop through array of triangle fans
            // this.shaderProgram.setUniformMatrix("transformationMatrix", array[i].createTransformMatrix());

            base.OnUpdateFrame(args);
        }

        // Initial frame render
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit); // Clear screen

            GL.UseProgram(this.shaderProgram.ShaderProgramHandle);
            GL.BindVertexArray(this.vertexArray.VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.indexBuffer.IndexBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, this.indexCount, DrawElementsType.UnsignedInt, 0);

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
