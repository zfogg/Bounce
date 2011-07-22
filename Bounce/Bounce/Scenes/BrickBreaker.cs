using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bounce.Scenes
{
    class BrickBreaker : Scene
    {
        private Camera2D camera;

        public BrickBreaker(Game game, Camera2D camera, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch )
            : base(game, camera, graphicsDevice, spriteBatch)
        {
            ItemStructures.CreateFraming(
                World, new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), 20);
            ItemFactory.CreateSamus(World);
            ItemFactory.CreatePaddleCenterFloor(World);
            //List<Obstacle> obstacles = ItemStructures.CreateRandomlyPositionedObstacles(World, windowSize, r.Next(5));
            //ItemStructures.MetroidsNearItems(World,
            //    obstacles.ConvertAll<PhysicalItem>(x => (PhysicalItem)x),
            //    Vector2.UnitY * 50f, 50);
            //ItemStructures.MetroidRow(World, 5, new Vector2(50, 189), 135);
            //ItemStructures.MetroidColumn(World, 5, new Vector2(windowSize.X / 2, 40), 80);

            this.camera = camera;
            Input.OnKeyHoldDown += new KeyboardEvent(OnKeyHoldDown);
        }

        public override void Update(GameTime gameTime)
        {
            World.Gravity.X = (float)Math.Sin(camera.Rotation) * GravityCoEf;
            World.Gravity.Y = (float)Math.Cos(camera.Rotation) * GravityCoEf;
            base.Update(gameTime);
        }

        void OnKeyHoldDown(KeyboardState keyboardState)
        {
            if (this.Enabled)
            {
                if (keyboardState.IsKeyDown(Keys.D1) && Input.LeftClickRelease())
                    ItemFactory.CreateMetroid(World, Input.MousePosition);

                if (keyboardState.IsKeyDown(Keys.D2) && Input.LeftClickRelease())
                    ItemFactory.CreateBrick(World, Input.MousePosition);

                if (keyboardState.IsKeyDown(Keys.D3) && Input.LeftClickRelease())
                    ItemFactory.CreateObstacle(World, Input.MousePosition);

                if (keyboardState.IsKeyDown(Keys.F10) && Input.LeftClickRelease())
                    ItemStructures.MetroidRow(World, 5, Input.MousePosition, 50);

                if (keyboardState.IsKeyDown(Keys.F11) && Input.LeftClickRelease())
                    ItemStructures.MetroidColumn(World, 5, Input.MousePosition, 50);

                if (keyboardState.IsKeyDown(Keys.F12) && Input.LeftClickRelease())
                    ItemStructures.BrickRow(World, 5, Input.MousePosition, 40);
            }
        }

        public override void Kill()
        {
            Input.OnKeyDown -= OnKeyHoldDown;
            base.Kill();
        }
    }
}
