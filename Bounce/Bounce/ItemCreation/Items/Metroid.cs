using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Controllers;


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
        public override void Update(GameTime gameTime) //$ idea 'make metroids hover when they near the ground.'
        {
            currentPosition = Body.Position;
            if (this.IsAlive)
            {
                if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
                {
                    if (InputHelper.KeyPressUnique(Keys.Space))
                    {
                        this.Body.IgnoreGravity = this.Body.IgnoreGravity ? false : true; //Toggle on/off
                        this.sinActive = sinActive ? false : true; //Toggle on/off
                        //if (sinRadius == 0)
                            this.sinRadius = (float)UnitCircle.Random(); //Random unit circle segment value, in radians
                        //if (cosRadius == 0)
                            this.cosRadius = (float)UnitCircle.Random(); //Random unit circle segment value, in radians
                        this.sinCenter = Body.Position;
                        this.sinCenter.X += (float)Math.Cos(UnitCircle.Random()); //To shift Cos, because a Cos wave = a 'shifted Sin' wave

                        Body.ApplyLinearImpulse(new Vector2(-cosRadius / (float)UnitCircle.TwoPi, sinRadius / (float)UnitCircle.PiOverOne)); //To get started.
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
                    sinCenter.Y -= sinCenter.Y - Body.Position.Y; //This moves the sprites sinCenter, if he gets pushed too far from it.
                if (Body.Position.X > sinCenter.X + distanceLimit || Body.Position.X < sinCenter.X - distanceLimit)
                    sinCenter.X -= sinCenter.X - Body.Position.X; //This moves the sprites sinCenter, if he gets pushed too far from it.
            }

            if (!this.IsAlive)
            {
                Body.Awake = true;
                Body.Dispose();
                BounceGame.PhysicalSprites.Remove(this);
            }

            previousPosition = currentPosition;
            base.Update(gameTime);
        }

        bool sinActive;
        Vector2 sinForce;
        float sinRadius, distanceLimit = 2f;
        private void SinMotion()
        {
            sinForce = Vector2.Zero;
            sinForce.Y = (float)Math.Sin(Body.Position.Y - sinCenter.Y); //Apply force of sin(current distance from the sum of the vertices of a revolution)
            Body.ApplyForce((-sinForce) * sinRadius);
            Body.ApplyForce(-Vector2.UnitY * BounceGame.World.Gravity); //Antigravity
        }

        float cosRadius;
        private void CosMotion()
        {
            sinForce = Vector2.Zero;
            sinForce.X = (float)Math.Sin(Body.Position.X - sinCenter.X);
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
