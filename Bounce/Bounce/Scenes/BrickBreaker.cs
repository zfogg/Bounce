using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    class BrickBreaker : PhysicalScene
    {
        override public Vector2 SceneSize
            { get { return new Vector2(sceneStack.GraphicsDevice.Viewport.Width, sceneStack.GraphicsDevice.Viewport.Height); } }
        private Camera2D camera;
        override public bool BlockDraw { get { return true; } }

        private List<RectangleItem> framing;
        private Paddle paddle;
        private PaddleBall ball;
        public List<Brick> bricks;

        public BrickBreaker(SceneStack sceneStack)
            : base(sceneStack)
        {
            this.camera = (Camera2D)sceneStack.Game.Services.GetService(typeof(Camera2D));
            background = new Background(this, Vector2.Zero, "background2");
            ConvertUnits.SetDisplayUnitToSimUnitRatio(90f);
            Input.OnKeyHoldDown += new KeyboardEvent(onKeyHoldDown);
        }

        public override void Initialize()
        {
            framing = ItemStructures.CreateFraming(this, SceneSize, 20);
            PhysicalItems.AddRange(framing);

            bricks = brickWall(10);
            PhysicalItems.AddRange(bricks);

            paddle = ItemFactory.CreatePaddle(this, SceneSize * (Vector2.UnitX / 2 + Vector2.UnitY * 0.95f));
            PhysicalItems.Add(paddle);

            ball = ItemFactory.CreatePaddleBall(this, paddle);
            PhysicalItems.Add(ball);

            killOnTouch<Brick>(ball);
        }

        private List<Brick> brickWall(int rows)
        {
            var sampleBrick = ItemFactory.CreateBrick(this);
            var bricks = new List<Brick>();

            var rowStartingPositions = VectorStructures.Column(rows,
                new Vector2(sampleBrick.Texture.Width / 2, sampleBrick.Texture.Height / 2),
                sampleBrick.Texture.Height);

            foreach (Vector2 startingPosition in rowStartingPositions)
            {
                bricks.AddRange(ItemStructures.BrickRow(this,
                    sceneStack.GraphicsDevice.Viewport.Width / sampleBrick.Texture.Width - 1,
                    startingPosition,
                    sampleBrick.Texture.Width));
            }

            sampleBrick.Kill();

            return bricks;
        }

        private void killOnTouch<T>(PhysicalItem killAfterTouching) where T : PhysicalItem
        {
            killAfterTouching.Body.OnCollision += (Fixture fixtureA, Fixture fixtureB, Contact contact) =>
            {
                if (fixtureB.Body.UserData is T)
                    (fixtureB.Body.UserData as T).Kill();

                return true;
            };
        }

        private void killAfterTouch<T>(PhysicalItem killAfterTouching) where T : PhysicalItem
        {
            killAfterTouching.Body.FixtureList[0].OnSeparation = (Fixture fixtureA, Fixture fixtureB) =>
            {
                if (fixtureB.Body.UserData is T)
                    (fixtureB.Body.UserData as T).Kill();
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (IsTop)
                Input.Update();

            worldGravityRotation(camera.Rotation);
            base.Update(gameTime);
        }

        void onKeyHoldDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.D1) && Input.LeftClickRelease())
                PhysicalItems.Add(ItemFactory.CreateMetroid(
                    this, Input.MouseVector2));
        }
    }
}