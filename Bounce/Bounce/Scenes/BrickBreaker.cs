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
        public World World { get; protected set; }
        private const float gravityCoEf = 5f;
        protected DebugBounce debugFarseer;

        private Camera2D camera;
        //override public bool BlockUpdate { get { return false; } }
        override public bool BlockDraw { get { return false; } }
        public Dictionary<IndexKey, PhysicalItem> PhysicalItems { get; private set; }
        protected List<PhysicalItem> itemsToKill;

        public BrickBreaker(SceneStack sceneStack, Camera2D camera, GraphicsDevice graphicsDevice)
            : base(sceneStack)
        {
            this.camera = camera;
            background = new Background(Vector2.Zero, "background2");
            World = new World(BounceGame.GravityCoEf * Vector2.UnitY);
            debugFarseer = new DebugBounce(World, camera);
            debugFarseer.Initialize(graphicsDevice, BounceGame.ContentManager);

            PhysicalItems = new Dictionary<IndexKey, PhysicalItem>();
            ItemFactory.ActiveDict = PhysicalItems;
            itemsToKill = new List<PhysicalItem>(ItemFactory.CreationLimit);

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

            Input.OnKeyHoldDown += new KeyboardEvent(OnKeyHoldDown);
        }

        public override void Update(GameTime gameTime)
        {
            Input.MouseHoverPhysicalItem(World);

            foreach (PhysicalItem item in PhysicalItems.Values)
            {
                if (item.IsAlive)
                    item.Update(gameTime);
                else
                    itemsToKill.Add(item);
            }

            foreach (PhysicalItem item in itemsToKill)
            {
                item.Kill();
                PhysicalItems.Remove(item.IndexKey);
            }

            itemsToKill.RemoveRange(0, itemsToKill.Count);

            World.Gravity.X = (float)Math.Sin(camera.Rotation) * gravityCoEf;
            World.Gravity.Y = (float)Math.Cos(camera.Rotation) * gravityCoEf;

            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            debugFarseer.Update(gameTime);
        }

        void OnKeyHoldDown(KeyboardState keyboardState)
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

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin(
                SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, null,
                camera.GetTransformation());

            foreach (PhysicalItem sprite in PhysicalItems.Values)
                sprite.Draw(spriteBatch);

            spriteBatch.End();

            debugFarseer.Draw();
        }

        public override void Kill()
        {

        }
    }
}
