using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    class BrickBreaker : PhysicalScene
    {
        override public Vector2 SceneSize { get { return new Vector2(sceneStack.GraphicsDevice.Viewport.Width, sceneStack.GraphicsDevice.Viewport.Height); } }
        private Camera2D camera;
        override public bool BlockDraw { get { return false; } }

        private List<RectangleItem> framing;
        private List<Obstacle> obstacles;
        private List<Metroid> metroids;
        private Samus samus;
        private Paddle paddle;
        private PaddleBall ball;
        public List<Brick> bricks;

        public BrickBreaker(SceneStack sceneStack, Camera2D camera)
            : base(sceneStack, camera)
        {
            this.camera = camera;
            background = new Background(this, Vector2.Zero, "background2");

            Input.OnKeyHoldDown += new KeyboardEvent(OnKeyHoldDown);
        }

        public override void Initialize()
        {
            base.Initialize();

            framing = ItemStructures.CreateFraming(
                World, new Vector2(sceneStack.GraphicsDevice.Viewport.Width, sceneStack.GraphicsDevice.Viewport.Height), 20);
            samus = ItemFactory.CreateSamus(World);

            paddle = ItemFactory.CreatePaddleCenterFloor(World);
            ball = ItemFactory.CreatePaddleBall(World, paddle);

            brickWall(10);
            destroyAfterTouch<Brick>(ball);
        }

        private List<Brick> brickWall(int rows)
        {
            var sampleBrick = ItemFactory.CreateBrick(World);
            var bricks = new List<Brick>();

            var rowStartingPositions = VectorStructures.Column(rows,
                new Vector2(sampleBrick.Texture.Width / 2, sampleBrick.Texture.Height / 2),
                sampleBrick.Texture.Height);

            foreach (Vector2 startingPosition in rowStartingPositions)
            {
                bricks.AddRange(ItemStructures.BrickRow(World,
                    sceneStack.GraphicsDevice.Viewport.Width / sampleBrick.Texture.Width - 1,
                    startingPosition,
                    sampleBrick.Texture.Width));
            }

            sampleBrick.Kill();

            return bricks;
        }

        private void destroyOnTouch<T>(PhysicalItem destroyOnTouching) where T:PhysicalItem
        {
            destroyOnTouching.Body.OnCollision += (Fixture fixtureA, Fixture fixtureB, Contact contact) =>
            {
                if (fixtureB.Body.UserData is T)
                    (fixtureB.Body.UserData as T).Kill();

                return true;
            };
        }

        private void destroyAfterTouch<T>(PhysicalItem destroyAfterTouching) where T : PhysicalItem
        {
            destroyAfterTouching.Body.FixtureList[0].OnSeparation = (Fixture fixtureA, Fixture fixtureB) =>
            {
                if (fixtureB.Body.UserData is T)
                    (fixtureB.Body.UserData as T).Kill();
            };
        }

        public override void Update(GameTime gameTime)
        {
            World.Gravity.X = (float)Math.Sin(camera.Rotation) * gravityCoEf;
            World.Gravity.Y = (float)Math.Cos(camera.Rotation) * gravityCoEf;

            base.Update(gameTime);
        }

        void OnKeyHoldDown(KeyboardState keyboardState)
        {
            if (IsTop)
            {
                if (keyboardState.IsKeyDown(Keys.D2) && Input.LeftClickRelease())
                     ItemFactory.CreateBrick(World, Input.MousePosition);

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