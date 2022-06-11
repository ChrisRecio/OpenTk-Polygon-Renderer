using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Collision_Simulation
{
    internal class Game : GameWindow
    {
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private VertexArray vertexArray;
        private ShaderProgram shaderProgram;

        private string vertexShaderLocation = "../../../assets/vertexShader.glsl";
        private string fragmentShaderLocation = "../../../assets/fragmentShader.glsl";

        private int vertexCount, indexCount;
        private float colorFactor = 1f, deltaColorFactor = 1f/256f;

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

            // Background Color
            GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));

            //pixelColor = vec4(1.0f, 0.5f, 0.31f, 1.0f); ORANGE

            Random rand = new Random();
            int boxCount = 100;
            int windowWidth = this.ClientSize.X;
            int windowHeight = this.ClientSize.Y;

            VertexPositionColor[] vertices = new VertexPositionColor[boxCount * 4];
            this.vertexCount = 0;

            for(int i = 0; i < boxCount; i++)
            {
                int w = rand.Next(32, 128);
                int h = rand.Next(32, 128);
                int x = rand.Next(0, windowWidth - w);
                int y = rand.Next(0, windowHeight - h);

                float r = (float)rand.NextDouble();
                float g = (float)rand.NextDouble();
                float b = (float)rand.NextDouble();


                vertices[this.vertexCount++] = new VertexPositionColor(new Vector2(x, y + h), new Color4(r, g, b, 1.0f));
                vertices[this.vertexCount++] = new VertexPositionColor(new Vector2(x + w, y + h), new Color4(r, g, b, 1.0f));
                vertices[this.vertexCount++] = new VertexPositionColor(new Vector2(x + w, y), new Color4(r, g, b, 1.0f));
                vertices[this.vertexCount++] = new VertexPositionColor(new Vector2(x, y), new Color4(r, g, b, 1.0f));
            }


            int[] indices = new int[boxCount * 6];
            this.indexCount = 0;
            this.vertexCount = 0;

            for (int i = 0; i < boxCount; i++)
            {
                indices[this.indexCount++] = 0 + this.vertexCount;
                indices[this.indexCount++] = 1 + this.vertexCount;
                indices[this.indexCount++] = 2 + this.vertexCount;
                indices[this.indexCount++] = 0 + this.vertexCount;
                indices[this.indexCount++] = 2 + this.vertexCount;
                indices[this.indexCount++] = 3 + this.vertexCount;

                this.vertexCount += 4;
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
