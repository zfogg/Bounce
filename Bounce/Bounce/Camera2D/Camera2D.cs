using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce
{
    public class Camera2D
    {
        public Camera2D()
        {
            Zoom = 1.0f;
            Rotation = 0.0f;
            Position = new Vector2(BounceGame.Graphics.GraphicsDevice.Viewport.Width * 0.5f, BounceGame.Graphics.GraphicsDevice.Viewport.Height * 0.5f);
        }

        public Vector2 Position;
        public Matrix Transform;
        public float Zoom;
        public float Rotation;
        private Vector2 movement;

        public void Update()
        {
            movement = Vector2.Zero;

            if (Input.IsNewState)
            {
                if (Input.KeyboardState.IsKeyDown(Keys.NumPad5)) //Reset fields
                {
                    Position = new Vector2(BounceGame.Graphics.GraphicsDevice.Viewport.Width * 0.5f, BounceGame.Graphics.GraphicsDevice.Viewport.Height * 0.5f);
                    Zoom = 1f;
                    Rotation = 0f;
                }
                
                if (Input.KeyboardState.IsKeyDown(Keys.NumPad8))
                    movement.Y += -1f;
                if (Input.KeyboardState.IsKeyDown(Keys.NumPad6))
                    movement.X += 1f;
                if (Input.KeyboardState.IsKeyDown(Keys.NumPad2))
                    movement.Y += 1f;
                if (Input.KeyboardState.IsKeyDown(Keys.NumPad4))
                    movement.X += -1f;

                if (movement != Vector2.Zero)
                    movement.Normalize();

                Position += movement * BounceGame.MovementCoEf;

                //Consider changing to exponential multiplication for zoom's value.
                if (Input.KeyboardState.IsKeyDown(Keys.Add))
                    Zoom += 0.025f;
                if (Input.KeyboardState.IsKeyDown(Keys.Subtract))
                    Zoom += -0.025f;

                if (Input.KeyboardState.IsKeyDown(Keys.PageUp))
                    Rotation += 0.025f;
                if (Input.KeyboardState.IsKeyDown(Keys.PageDown))
                    Rotation += -0.025f;
            }
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            Transform =
                Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));

            return Transform;
        }
    }
}
