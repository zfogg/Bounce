using System.Collections.Generic;
using System.Linq;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    class BrickBreakerScene : PhysicalScene
    {
        override public Vector2 SceneSize { get { return new Vector2(sceneStack.GraphicsDevice.Viewport.Width, sceneStack.GraphicsDevice.Viewport.Height); } }
        override public bool BlockDraw { get { return true; } }

        private List<RectangleItem> framing;
        private Paddle paddle;
        private PaddleBall ball;

        private Score score;
        private SpriteFont scoreFont;
        private string scoreStringFormat = "Score: {0:0.}";
        private string scoreString = "";
        private Vector2 scorePosition;

        public BrickBreakerScene(SceneStack sceneStack)
            : base(sceneStack)
        {
            background = new Background(Vector2.Zero, "space3");
            Input.OnKeyDown += new KeyboardEvent(onKeyDown);
            Input.OnKeyHoldDown += new KeyboardEvent(onKeyHoldDown);

            score = new Score(1f);
            scorePosition = SceneSize * (Vector2.UnitX * 0.015f + Vector2.UnitY * 0.95f);
        }

        public override void Initialize()
        {
            framing = ItemStructures.CreateFraming(this, SceneSize, width: 20);
            PhysicalItems.AddRange(framing);

            var bricks = brickWall(8);
            //bricks = orderByColor<Brick>(bricks).ToList();
            PhysicalItems.AddRange(bricks);

            paddle = ItemFactory.CreatePaddle(this, SceneSize * (Vector2.UnitX / 2f + Vector2.UnitY * 0.95f));
            PhysicalItems.Add(paddle);

            ball = ItemFactory.CreatePaddleBall(this, paddle);
            PhysicalItems.Add(ball);

            ball.KillOnTouch<Brick>();
            ball.Body.OnCollision += new OnCollisionEventHandler(incrementScore);

            // [1] is the RectangleItem under the paddle.
            framing[1].Body.OnCollision += new OnCollisionEventHandler(resetBall);

            scoreFont = BounceGame.ContentManager.Load<SpriteFont>("arial");
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (sceneStack.Top == this)
                Input.Update();

            if (PhysicalItems.Find(x => x is Brick) == null)
            {
                var nextLevel = new BrickBreakerScene(sceneStack);
                sceneStack.Push(new TimedTransitionScene(sceneStack, nextLevel, 3.5f, "We need to go deeper."));
            }

            scoreString = string.Format(scoreStringFormat, score.TotalScore * 100f);

            worldGravityRotation(camera.Rotation);
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                scoreFont,
                scoreString,
                scorePosition,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                1f);

            background.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        List<Brick> brickWall(int rows)
        {
            var sampleBrick = ItemFactory.CreateBrick(this);

            var rowStartingPositions = VectorStructures.Column(
                rows, new Vector2(sampleBrick.Texture.Width / 2, sampleBrick.Texture.Height / 2), sampleBrick.Texture.Height);

            var bricks = new List<Brick>();
            foreach (Vector2 startingPosition in rowStartingPositions)
            {
                var newRow = ItemStructures.BrickRow(
                    this, (int)SceneSize.X / sampleBrick.Texture.Width - 1, startingPosition, sampleBrick.Texture.Width);

                bricks.AddRange(newRow);
            }

            sampleBrick.Kill();

            return bricks;
        }

        IEnumerable<T> orderByColor<T>(IEnumerable<T> unorderedItems) where T : PhysicalItem
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

        bool resetBall(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData == ball)
                resetBall();

            return true;
        }

        void resetBall()
        {
            ball.FixToPaddle(ConvertUnits.ToSimUnits(Vector2.UnitY * ball.Texture.Height));
            score.ResetMultiplier();
        }

        bool incrementScore(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData is Brick)
            {
                score.Increment(0.1f);
                score.Multiplier *= 1.025f;
            }

            return true;
        }

        void onKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Q))
                orderByColor(PhysicalItems.FindAll(b => b is Brick));
        }

        void onKeyHoldDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.D1) && Input.LeftClickRelease())
                PhysicalItems.Add(ItemFactory.CreateMetroid(this, Input.MouseVector2));
            else if (keyboardState.IsKeyDown(Keys.D2) && Input.LeftClickRelease())
                PhysicalItems.Add(new LineItem(this, ConvertUnits.ToSimUnits(Input.MouseVector2), 0.25f, 0.001f));
        }
    }
}