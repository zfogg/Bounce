using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.DebugViews;

namespace Bounce
{
    public class DebugBounce
    {
        private DebugViewXNA DebugViewXNA;
        private Matrix projection, view;
        private GraphicsDevice graphicsDevice;
        private Camera2D camera;

        public DebugBounce(World world, Camera2D camera)
        {   
            DebugViewXNA = new DebugViewXNA(world);
            this.camera = camera;
        }

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager, Input input)
        {
            this.graphicsDevice = graphicsDevice;

            DebugViewXNA.LoadContent(graphicsDevice, contentManager);
            DebugViewXNA.RemoveFlags(DebugViewFlags.Shape);

            projection = Matrix.CreateOrthographic(
                graphicsDevice.Viewport.Width / 100.0f,
                -graphicsDevice.Viewport.Height / 100.0f, 0, 1000000);

            Vector3 campos = new Vector3();
            campos.X = (-graphicsDevice.Viewport.Width / 2) / 100.0f;
            campos.Y = (graphicsDevice.Viewport.Height / 2) / -100.0f;
            campos.Z = 0;
            Matrix tran = Matrix.Identity;
            tran.Translation = campos;
            view = tran;

            input.OnKeyDown += OnKeyDown;
        }

        public void Update(GameTime gameTime)
        {
            // Projection (zoom)
            float width = (1f / camera.Zoom) * ConvertUnits.ToSimUnits(graphicsDevice.Viewport.Width / 2);
            float height = (-1f / camera.Zoom) * ConvertUnits.ToSimUnits(graphicsDevice.Viewport.Height / 2);
            //projection = Matrix.CreateOrthographic(width, height, 1f, 1000000f);
            projection = Matrix.CreateOrthographicOffCenter(
                -width,
                width,
                -height,
                height,
                0f, 1000000f);

            // View (translation and rotation)
            float xTranslation = -1 * ConvertUnits.ToSimUnits(camera.Position.X);
            float yTranslation = -1 * ConvertUnits.ToSimUnits(camera.Position.Y);
            Vector3 translationVector = new Vector3(xTranslation, yTranslation, 0f);
            view = Matrix.CreateRotationZ(camera.Rotation);
            view.Translation = translationVector;
        }

        public void OnKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.F1))
                EnableOrDisableFlag(DebugViewFlags.Shape);

            if (keyboardState.IsKeyDown(Keys.F2))
            {
                EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
            }
            
            if (keyboardState.IsKeyDown(Keys.F3))
                EnableOrDisableFlag(DebugViewFlags.Joint);

            if (keyboardState.IsKeyDown(Keys.F4))
            {
                EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                EnableOrDisableFlag(DebugViewFlags.ContactNormals);
            }
            
            if (keyboardState.IsKeyDown(Keys.F5))
                EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
            
            if (keyboardState.IsKeyDown(Keys.F6))
                EnableOrDisableFlag(DebugViewFlags.Controllers);
            
            if (keyboardState.IsKeyDown(Keys.F7))
                EnableOrDisableFlag(DebugViewFlags.CenterOfMass);

            if (keyboardState.IsKeyDown(Keys.F8))
                EnableOrDisableFlag(DebugViewFlags.AABB);

            if (keyboardState.IsKeyDown(Keys.F9))
                EnableOrDisableFlag(DebugViewFlags.Pair);
        }

        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugViewXNA.Flags & flag) == flag)
                DebugViewXNA.RemoveFlags(flag);
            else
                DebugViewXNA.AppendFlags(flag);
        }

        public void Draw()
        {
            DebugViewXNA.RenderDebugData(ref projection, ref view);
        }

        public void Kill()
        {
            DebugViewXNA.Enabled = false;
            DebugViewXNA.Dispose();
        }
    }
}
