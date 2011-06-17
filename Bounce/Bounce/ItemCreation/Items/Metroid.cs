using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Metroid : PhysicalSprite
    {
        public Metroid(Game game, float sinRadius, float cosRadius)
            : this(game)
        {
            this.cosRadius = cosRadius;
            this.sinRadius = sinRadius;
        }
        public Metroid(Game game)
            : base(game)
        {
            this.IsAlive = true;

            if (Texture == null)
                Texture = Game.Content.Load<Texture2D>("metroid");

            Body = BodyFactory.CreateCircle(BounceGame.World,
                        ConvertUnits.ToSimUnits(this.Texture.Width / 2),
                        ConvertUnits.ToSimUnits(this.Texture.Height / 2), 1); ;
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 1f;
            Body.Friction = 0.25f;
            Body.Restitution = .35f;
            Body.AngularDamping = 0.075f;
            Body.IgnoreGravity = true;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }
        public override void Initialize()
        {
            BounceGame.PhysicalSprites.Add(this);
            base.Initialize();
        }


        Vector2 currentPosition, previousPosition;
        Vector2 force = Vector2.Zero;
        public override void Update(GameTime gameTime) //make metroids hover when they near the ground.
        {
            currentPosition = Body.Position;
            if (this.IsAlive)
            {
                if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
                {
                    if (InputHelper.KeyPressUnique(Keys.Space)) //Caution: experimental, horribly messy, and convoluted.
                    {
                        this.Body.IgnoreGravity = this.Body.IgnoreGravity ? false : true;
                        this.sinActive = sinActive ? false : true;
                        this.sinRadius = (float)UnitCircle.RandomSegment();
                        this.cosRadius = (float)UnitCircle.RandomSegment();
                        this.sinCenter = Body.Position;
                        this.sinCenter.X += this.cosRadius / 4f;
                        //7:14pm 7/16/11 - For some reason I'm getting inside-out sin and/or cos motion, such as that the sprite is at its slowest at point (0, 0).
                        //Don't use UnitCircle.RandomSign() until I fix that.
                        Body.ApplyLinearImpulse(new Vector2(cosRadius / 4f, sinRadius / 2f));
                    }
                }


                if (BounceGame.MouseState != BounceGame.PreviousMouseState)
                {
                    if (InputHelper.RickClickRelease())
                        this.IsAlive = false;
                }
            }

            if (sinActive)
            {
                SinMotion();
                CosMotion();

                if (Body.Position.Y > sinCenter.Y + distanceLimit || Body.Position.Y < sinCenter.Y - distanceLimit)
                    sinCenter.Y -= sinCenter.Y - Body.Position.Y;
                if (Body.Position.X > sinCenter.X + distanceLimit || Body.Position.X < sinCenter.X - distanceLimit)
                    sinCenter.X -= sinCenter.X - Body.Position.X;
            }

            if (!this.IsAlive)
            {
                Body.Dispose();
                BounceGame.PhysicalSprites.Remove(this);
            }

            previousPosition = currentPosition;
            base.Update(gameTime);
        }

        bool sinActive;
        Vector2 sinForce;
        float sinRadius, distanceLimit = 2f;
        private void SinMotion() //Messy and experimental.
        {
            sinForce = Vector2.Zero;
            sinForce.Y = (float)Math.Sin(Body.Position.Y - sinCenter.Y); //Apply force of sin(current distance from the sum of the vertices of a revolution)
            Body.ApplyForce((-sinForce) * sinRadius);
            Body.ApplyForce(-Vector2.UnitY * BounceGame.World.Gravity);
        }

        float cosRadius;
        private void CosMotion() //Messy and experimental.
        {
            sinForce = Vector2.Zero;
            sinForce.X = (float)Math.Sin((Body.Position.X - sinCenter.X));
            Body.ApplyForce((-sinForce) * cosRadius);
        }
        
        public override void Draw()
        {
            BounceGame.SpriteBatch.Draw(Texture,
                ConvertUnits.ToDisplayUnits(Body.Position),
                null, Color.White,
                Body.Rotation,
                origin,
                1f,
                SpriteEffects.None,
                0);
        }
    }
}
