using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Metroid : PhysicalItem
    {
        public Metroid(World world, float sinRadius, float cosRadius)
            : this(world)
        {
            this.cosRadius = cosRadius;
            this.sinRadius = sinRadius;
        }

        public Metroid(World world)
            : base(world)
        {
            this.IsAlive = true;
            Texture = BounceGame.ContentManager.Load<Texture2D>("metroid");

            unitCircle = new UnitCircle();
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Body = BodyFactory.CreateCircle(world,
                        ConvertUnits.ToSimUnits(this.Texture.Width / 2),
                        ConvertUnits.ToSimUnits(this.Texture.Height / 2), 1);

            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 1f;
            Body.Friction = 0.25f;
            Body.Restitution = .35f;
            Body.AngularDamping = 0.075f;
            Body.IgnoreGravity = true;

            Body.OnCollision += new OnCollisionEventHandler(OnCollision);
        }

        private UnitCircle unitCircle;
        private Vector2 force = Vector2.Zero;

        public override void Update(GameTime gameTime) // Idea: make metroids hover when they near the ground.
        {
            if (Input.IsNewState)
            {
                if (Input.KeyPressUnique(Keys.Space)) //Caution: experimental, horribly messy, and convoluted.
                {
                    sinActive = !sinActive;

                    if (sinRadius == 0 && cosRadius == 0)
                    {
                        sinRadius = (float)unitCircle.RandomSegment();
                        cosRadius = (float)unitCircle.RandomSegment();
                    }

                    sinCenter = Body.Position;
                    sinCenter.X += (float)Math.Sin((double)cosRadius);

                    Body.ApplyLinearImpulse(new Vector2((float)cosRadius / 2f, (float)sinRadius / 2f));
                }


                if (Input.MiddleClickUnique())
                {
                    Body.Awake = true;
                    Body.IgnoreGravity = !Body.IgnoreGravity;
                }

                if (Input.RickClickRelease())
                    IsAlive = false;
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
        }

        private bool sinActive;
        private Vector2 sinForce, sinCenter;
        private float sinRadius, distanceLimit = 2f;
        private void SinMotion() //Messy and experimental.
        {
            sinForce = Vector2.Zero;
            sinForce.Y = (float)Math.Sin(Body.Position.Y - sinCenter.Y); //Apply force of sin(current distance from the sum of the vertices of a revolution)
            Body.ApplyForce((-sinForce) * sinRadius);
        }

        float cosRadius;
        private void CosMotion() //Messy and experimental.
        {
            sinForce = Vector2.Zero;
            sinForce.X = (float)Math.Sin((Body.Position.X - sinCenter.X));
            Body.ApplyForce((-sinForce) * cosRadius);
        }

        public bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.Body.UserData == typeof(Rectangle))
                this.IsAlive = false;

            return true;
        }
    }
}
