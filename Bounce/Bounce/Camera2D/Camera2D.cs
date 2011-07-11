﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce
{
    class Camera2D// : GameComponent
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

        public void Update(KeyboardState keyboardState)
        {
            movement = Vector2.Zero;

            if (keyboardState.GetPressedKeys().Length != 0)
            {
                if (keyboardState.IsKeyDown(Keys.NumPad5)) //Reset fields
                {
                    Position = new Vector2(BounceGame.Graphics.GraphicsDevice.Viewport.Width * 0.5f, BounceGame.Graphics.GraphicsDevice.Viewport.Height * 0.5f);
                    Zoom = 1f;
                    Rotation = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.NumPad8))
                    movement.Y += -1f;
                if (keyboardState.IsKeyDown(Keys.NumPad6))
                    movement.X += 1f;
                if (keyboardState.IsKeyDown(Keys.NumPad2))
                    movement.Y += 1f;
                if (keyboardState.IsKeyDown(Keys.NumPad4))
                    movement.X += -1f;

                if (movement != Vector2.Zero)
                    movement.Normalize();

                Position += movement * BounceGame.MovementCoEf;

                //Consider changing to exponential multiplication for zoom's value.
                if (keyboardState.IsKeyDown(Keys.Add))
                    Zoom += 0.025f;
                if (keyboardState.IsKeyDown(Keys.Subtract))
                    Zoom += -0.025f;

                //To do: implement camera rotation around the Z axis.
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