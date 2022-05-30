using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Collision_Simulation
{
    internal class Game : GameWindow
    {
        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(1280, 768));
        }

        // Called per frame update
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        // Initial frame render
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(new Color4(1.0f, 0.5f, 0.31f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit);

            this.Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

    }
}
