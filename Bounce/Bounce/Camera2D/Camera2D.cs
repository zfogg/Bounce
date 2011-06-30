using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce
{
    class Camera2D// : GameComponent
    {
        protected float zoom;
        protected float rotation;
        public Camera2D(GraphicsDevice graphicsDevice)
        {
            zoom = 1.0f;
            rotation = 0.0f;
            Position = new Vector2(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f);
        }

        private Vector2 cameraPosition;
        public Matrix Transform;
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        private Vector2 movement;

        public void Step(float movementCoEf)
        {
            movement = Vector2.Zero;

            if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
            {
                if (BounceGame.KeyboardState.IsKeyDown((Keys.NumPad8)))
                    movement.Y += -1f;
                if (BounceGame.KeyboardState.IsKeyDown((Keys.NumPad6)))
                    movement.X += 1f;
                if (BounceGame.KeyboardState.IsKeyDown((Keys.NumPad2)))
                    movement.Y += 1f;
                if (BounceGame.KeyboardState.IsKeyDown((Keys.NumPad4)))
                    movement.X += -1f;

                if (movement != Vector2.Zero)
                    movement.Normalize();

                Position += movement * movementCoEf;

                //Consider changing to exponential multiplication for zoom's value.
                if (BounceGame.KeyboardState.IsKeyDown(Keys.Add))
                    zoom += 0.025f;
                if (BounceGame.KeyboardState.IsKeyDown(Keys.Subtract))
                    zoom += -0.025f;

                //To do: implement camera rotation around the Z axis.
            }
        }

        public Vector2 Position
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            Transform =
                Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));

            return Transform;
        }
    }
}
