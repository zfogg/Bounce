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
        DebugViewXNA DebugViewXNA;
        public DebugBounce(World world, GraphicsDevice graphicsDevice, Camera2D camera)
        {
            this.graphicsDevice = graphicsDevice;
            this.camera = camera;
            DebugViewXNA = new DebugViewXNA(world);
        }

        private Matrix projection, view;
        private GraphicsDevice graphicsDevice;
        private Camera2D camera;

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
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

            Input.OnKeyDown += OnKeyDown;
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
            if (Input.KeyPressUnique(Keys.F1))
                EnableOrDisableFlag(DebugViewFlags.Shape);

            if (Input.KeyPressUnique(Keys.F2))
            {
                EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
            }
            
            if (Input.KeyPressUnique(Keys.F3))
                EnableOrDisableFlag(DebugViewFlags.Joint);

            if (Input.KeyPressUnique(Keys.F4))
            {
                EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                EnableOrDisableFlag(DebugViewFlags.ContactNormals);
            }
            
            if (Input.KeyPressUnique(Keys.F5))
                EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
            
            if (Input.KeyPressUnique(Keys.F6))
                EnableOrDisableFlag(DebugViewFlags.Controllers);
            
            if (Input.KeyPressUnique(Keys.F7))
                EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
            
            if (Input.KeyPressUnique(Keys.F8))
                EnableOrDisableFlag(DebugViewFlags.AABB);
        }

        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugViewXNA.Flags & flag) == flag)
                DebugViewXNA.RemoveFlags(flag);
            else
                DebugViewXNA.AppendFlags(flag);
        }

        public void Draw(Camera2D camera, GraphicsDevice graphicsDevice)
        {
            DebugViewXNA.RenderDebugData(ref projection, ref view);
        }
    }
}
