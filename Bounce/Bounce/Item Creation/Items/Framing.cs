using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Framing : Microsoft.Xna.Framework.GameComponent
    {
        public Framing(Game game)
            : base(game)
        {
            BackgroundTexture = BounceGame.ContentManager.Load<Texture2D>("background");
            FloorTexture = BounceGame.ContentManager.Load<Texture2D>("floor");
            CielingTexture = BounceGame.ContentManager.Load<Texture2D>("floor");
            RightSideTexture = BounceGame.ContentManager.Load<Texture2D>("side");
            LeftSideTexture = BounceGame.ContentManager.Load<Texture2D>("side");
            backgroundPosition = Vector2.Zero;

            FloorBody = BodyFactory.CreateRectangle(BounceGame.World,
                ConvertUnits.ToSimUnits(FloorTexture.Width),
                ConvertUnits.ToSimUnits(FloorTexture.Height), 1f);
            FloorBody.BodyType = BodyType.Static;
            FloorBody.Position = new Vector2(
                ConvertUnits.ToSimUnits(0 + FloorTexture.Width / 2),
                ConvertUnits.ToSimUnits(BounceGame.Graphics.PreferredBackBufferHeight + (FloorTexture.Height / 2))
                );
            FloorBody.Restitution = 0.7f;
            floorOrigin = new Vector2(FloorTexture.Width / 2, FloorTexture.Height / 2);

            CielingBody = BodyFactory.CreateRectangle(BounceGame.World,
                ConvertUnits.ToSimUnits(CielingTexture.Width),
                ConvertUnits.ToSimUnits(CielingTexture.Height),
                1f);
            CielingBody.BodyType = BodyType.Static;
            CielingBody.Position = new Vector2(
                ConvertUnits.ToSimUnits(0 + CielingTexture.Width / 2),
                ConvertUnits.ToSimUnits(0 + -CielingTexture.Height / 2)
                );
            cielingOrigin = new Vector2(CielingTexture.Width / 2, CielingTexture.Height / 2);

            RightSideBody = BodyFactory.CreateRectangle(BounceGame.World,
                ConvertUnits.ToSimUnits(RightSideTexture.Width),
                ConvertUnits.ToSimUnits(RightSideTexture.Height),
                1f);
            RightSideBody.BodyType = BodyType.Static;
            RightSideBody.Position = new Vector2(
                ConvertUnits.ToSimUnits(0 + (-RightSideTexture.Width / 2)),
                ConvertUnits.ToSimUnits(0 + (RightSideTexture.Height / 2)));
            rightSideOrigin = new Vector2(RightSideTexture.Width / 2, RightSideTexture.Height / 2);

            LeftSideBody = BodyFactory.CreateRectangle(BounceGame.World,
                ConvertUnits.ToSimUnits(LeftSideTexture.Width),
                ConvertUnits.ToSimUnits(LeftSideTexture.Height),
                1f);
            LeftSideBody.BodyType = BodyType.Static;
            LeftSideBody.Position = new Vector2(
                ConvertUnits.ToSimUnits(0 + BounceGame.Graphics.PreferredBackBufferWidth - -(LeftSideTexture.Width / 2)),
                ConvertUnits.ToSimUnits(0 + BounceGame.Graphics.PreferredBackBufferHeight - (LeftSideTexture.Height / 2)));
            leftSideOrigin = new Vector2(LeftSideTexture.Width / 2, LeftSideTexture.Height / 2);
                

            game.Components.Add(this);
        }

        public static Texture2D BackgroundTexture, FloorTexture, CielingTexture,
                                                   RightSideTexture, LeftSideTexture;
        public static Body FloorBody, CielingBody,
                           RightSideBody, LeftSideBody;
        private Vector2 floorOrigin, cielingOrigin,
                        rightSideOrigin, leftSideOrigin;

        private Vector2 backgroundPosition;
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void Draw()
        {
            BounceGame.SpriteBatch.Draw(BackgroundTexture, backgroundPosition, Color.White);

            BounceGame.SpriteBatch.Draw(CielingTexture,
                ConvertUnits.ToDisplayUnits(CielingBody.Position),
                null,
                Color.White,
                CielingBody.Rotation,
                cielingOrigin,
                1f,
                SpriteEffects.FlipVertically, 1);

            BounceGame.SpriteBatch.Draw(RightSideTexture,
                ConvertUnits.ToDisplayUnits(RightSideBody.Position),
                null,
                Color.White,
                RightSideBody.Rotation,
                rightSideOrigin,
                1f,
                SpriteEffects.FlipHorizontally, 1);

            BounceGame.SpriteBatch.Draw(LeftSideTexture,
                ConvertUnits.ToDisplayUnits(LeftSideBody.Position),
                null,
                Color.White,
                LeftSideBody.Rotation,
                leftSideOrigin,
                1f,
                SpriteEffects.FlipVertically, 1);

            BounceGame.SpriteBatch.Draw(FloorTexture,
                ConvertUnits.ToDisplayUnits(FloorBody.Position),
                null,
                Color.White,
                FloorBody.Rotation,
                floorOrigin,
                1f,
                SpriteEffects.None, 1);
        }
    }
}
