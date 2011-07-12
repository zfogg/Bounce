using System;
using Microsoft.Xna.Framework;
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
        public DebugBounce(World world)
        {
            DebugViewXNA = new DebugViewXNA(world);
        }

        private Matrix projection, view;

        public void Initialize()
        {
            DebugViewXNA.LoadContent(BounceGame.Graphics.GraphicsDevice, BounceGame.ContentManager);
            DebugViewXNA.RemoveFlags(DebugViewFlags.Shape);

            projection = Matrix.CreateOrthographic(
                BounceGame.Graphics.PreferredBackBufferWidth / 100.0f,
                -BounceGame.Graphics.PreferredBackBufferHeight / 100.0f, 0, 1000000);

            Vector3 campos = new Vector3();
            campos.X = (-BounceGame.Graphics.PreferredBackBufferWidth / 2) / 100.0f;
            campos.Y = (BounceGame.Graphics.PreferredBackBufferHeight / 2) / -100.0f;
            campos.Z = 0;
            Matrix tran = Matrix.Identity;
            tran.Translation = campos;
            view = tran;
        }

        public void Update(GameTime gameTime)
        {
            if (Input.IsNewState)
            {
                if (Input.KeyPressUnique(Keys.F1))
                {
                    EnableOrDisableFlag(DebugViewFlags.Shape);
                }
                if (Input.KeyPressUnique(Keys.F2))
                {
                    EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                    EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
                }
                if (Input.KeyPressUnique(Keys.F3))
                {
                    EnableOrDisableFlag(DebugViewFlags.Joint);
                }
                if (Input.KeyPressUnique(Keys.F4))
                {
                    EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                    EnableOrDisableFlag(DebugViewFlags.ContactNormals);
                }
                if (Input.KeyPressUnique(Keys.F5))
                {
                    EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
                }
                if (Input.KeyPressUnique(Keys.F6))
                {
                    EnableOrDisableFlag(DebugViewFlags.Controllers);
                }
                if (Input.KeyPressUnique(Keys.F7))
                {
                    EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
                }
                if (Input.KeyPressUnique(Keys.F8))
                {
                    EnableOrDisableFlag(DebugViewFlags.AABB);
                }
            }
        }

        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugViewXNA.Flags & flag) == flag)
            {
                DebugViewXNA.RemoveFlags(flag);
            }
            else
            {
                DebugViewXNA.AppendFlags(flag);
            }
        }

        public void Draw(Camera2D camera, GraphicsDevice graphicsDevice)
        {
            // Projection (location and zoom)
            float width = (1f / camera.Zoom) * ConvertUnits.ToSimUnits(graphicsDevice.Viewport.Width);
            float height = (-1f / camera.Zoom) * ConvertUnits.ToSimUnits(graphicsDevice.Viewport.Height);
            float zNearPlane = 0f;
            float zFarPlane = 1000000f;
            projection = Matrix.CreateOrthographic(width, height, zNearPlane, zFarPlane);

            // View (translation and rotation)
            float xTranslation = -1 * ConvertUnits.ToSimUnits(camera.Position.X);
            float yTranslation = -1 * ConvertUnits.ToSimUnits(camera.Position.Y);
            Vector3 translationVector = new Vector3(xTranslation, yTranslation, 0f);
            view = Matrix.Identity * (Matrix.CreateRotationZ(camera.Rotation));
            view.Translation = translationVector;

            DebugViewXNA.RenderDebugData(ref projection, ref view);
        }
    }
}
