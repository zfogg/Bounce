using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce
{
    public class Camera2D
    {
        private GraphicsDevice graphicsDevice;
        public Vector2 Position;
        public Matrix Transform;
        public float Zoom;
        public float Rotation;
        private Vector2 movement;

        public Camera2D(GraphicsDevice graphicsDevice, Input input)
        {
            Zoom = 1.0f;
            Rotation = 0.0f;
            Position = new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f);
            this.graphicsDevice = graphicsDevice;

            input.OnKeyDown += new KeyboardEvent(onKeyDown);
            input.OnKeyHoldDown += new KeyboardEvent(onKeyHoldDown);
            input.OnKeyUp += new KeyboardEvent(onKeyUp);
        }

        void onKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.NumPad5)) //Reset fields
            {
                //Position = new Vector2(BounceGame.Graphics.GraphicsDevice.Viewport.Width * 0.5f, BounceGame.Graphics.GraphicsDevice.Viewport.Height * 0.5f);
                Zoom = 1f;
                Rotation = 0f;
            }
        }

        void onKeyHoldDown(KeyboardState keyboardState)
        {
            //Move
            if (keyboardState.IsKeyDown(Keys.NumPad8))
                movement.Y += -1f;
            if (keyboardState.IsKeyDown(Keys.NumPad6))
                movement.X += 1f;
            if (keyboardState.IsKeyDown(Keys.NumPad2))
                movement.Y += 1f;
            if (keyboardState.IsKeyDown(Keys.NumPad4))
                movement.X += -1f;

            if (movement != Vector2.Zero)
                Position += Vector2.Normalize(movement) * BounceGame.MovementCoEf;

            //Zoom
            if (keyboardState.IsKeyDown(Keys.Add))
                Zoom += 0.025f;
            if (keyboardState.IsKeyDown(Keys.Subtract))
                Zoom += -0.025f;

            //Rotate
            if (keyboardState.IsKeyDown(Keys.PageUp))
                Rotation += 0.025f;
            if (keyboardState.IsKeyDown(Keys.PageDown))
                Rotation += -0.025f;
        }

        void onKeyUp(KeyboardState previousKeyboardState)
        {
            movement = Vector2.Zero;
        }

        public Matrix GetTransformation()
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
