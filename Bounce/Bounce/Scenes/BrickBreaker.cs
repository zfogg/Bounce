using System;
using System.Linq;
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
        override public Vector2 SceneSize{ get { return new Vector2(sceneStack.GraphicsDevice.Viewport.Width, sceneStack.GraphicsDevice.Viewport.Height); } }
        override public bool BlockDraw { get { return true; } }

        private List<RectangleItem> framing;
        private Paddle paddle;
        private PaddleBall ball;

        public BrickBreaker(SceneStack sceneStack)
            : base(sceneStack)
        {
            background = new Background(this, Vector2.Zero, "space3");
            ConvertUnits.SetDisplayUnitToSimUnitRatio(90f);
            Input.OnKeyHoldDown += new KeyboardEvent(onKeyHoldDown);
            Input.OnKeyDown += new KeyboardEvent(Input_OnKeyDown);
        }

        void Input_OnKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Q))
                orderByColor(PhysicalItems.FindAll(b => b.GetType() == typeof(Brick)));
        }

        public override void Initialize()
        {
            framing = ItemStructures.CreateFraming(this, SceneSize, width: 20);
            PhysicalItems.AddRange(framing);

            var bricks = brickWall(8);
            //bricks = orderByColor<Brick>(bricks).ToList();
            PhysicalItems.AddRange(bricks);

            var killBrick = bricks.ToList<Brick>()[BounceGame.r.Next(bricks.Count<Brick>())];
            killBrick.DrawColor = Color.Black;
            killBrick.Body.OnCollision += (Fixture fixtureA, Fixture fixtureB, Contact contact) =>
                {
                    if (fixtureB.Body.UserData == ball)
                        sceneStack.Push(new TransitionScene(
                            sceneStack, new BrickBreaker(sceneStack), 10f, "Transitioning: "));

                    return true;
                };

            paddle = ItemFactory.CreatePaddle(this, SceneSize * (Vector2.UnitX / 2 + Vector2.UnitY * 0.95f));
            PhysicalItems.Add(paddle);

            ball = ItemFactory.CreatePaddleBall(this, paddle);
            PhysicalItems.Add(ball);

            killOnTouch<Brick>(ball);

            // [1] is the RectangleItem under the paddle.
            framing[1].Body.OnCollision += new OnCollisionEventHandler(resetBallOnFloor);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsTop)
                Input.Update();

            for (int i = 0; i < PhysicalItems.Count; i++)
            {
                if (PhysicalItems[i] is Brick)
                    break;
                else if (i + 1 == PhysicalItems.Count)
                    sceneStack.Push(new BrickBreaker(sceneStack)); // "We need to go deeper."
            }

            worldGravityRotation(camera.Rotation);
            base.Update(gameTime);
        }

        private List<Brick> brickWall(int rows)
        {
            var sampleBrick = ItemFactory.CreateBrick(this);

            var rowStartingPositions = VectorStructures.Column(
                rows,
                new Vector2(sampleBrick.Texture.Width / 2, sampleBrick.Texture.Height / 2),
                sampleBrick.Texture.Height);

            var bricks = new List<Brick>();
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

        private IEnumerable<T> orderByColor<T>(IEnumerable<T> unorderedItems) where T : PhysicalItem
        {
            var itemsByPosition = (from item in unorderedItems
                                   orderby item.Body.Position.Y, item.Body.Position.X
                                   select item).ToArray();

            var itemsByColor = (from item in itemsByPosition // Currently broken.
                                orderby item.DrawColor.R, item.DrawColor.G, item.DrawColor.B
                                select item).ToArray();

            for (int i = 0; i < unorderedItems.Count(); i++)
                itemsByPosition[i].Body.Position = itemsByColor[i].Body.Position;

            return unorderedItems;
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

        void onKeyHoldDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.D1) && Input.LeftClickRelease())
                PhysicalItems.Add(ItemFactory.CreateMetroid(
                    this, Input.MouseVector2));
            else if (keyboardState.IsKeyDown(Keys.D2) && Input.LeftClickRelease())
                PhysicalItems.Add(new LineItem(
                    this, ConvertUnits.ToSimUnits(Input.MouseVector2), 0.25f, 0.001f));
        }

        bool resetBallOnFloor(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body == ball.Body)
                ball.FixToPaddle(ConvertUnits.ToSimUnits(
                    Vector2.UnitY * ball.Texture.Height));

            return true;
        }
    }
}