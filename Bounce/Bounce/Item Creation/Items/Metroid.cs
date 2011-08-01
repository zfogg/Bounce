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
    public class Metroid : CircleItem
    {

        private UnitCircle unitCircle;
        private Vector2 force = Vector2.Zero;
        public Vector2 MotionRadius;

        bool sinActive;
        Vector2 sinForce, sinCenter;
        float distanceLimit = 2f;

        public Metroid(PhysicalScene scene, Texture2D texture)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width / 2))
        {
            this.Texture = texture;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            unitCircle = new UnitCircle();

            Body.IgnoreGravity = true;
            
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 1f;
            Body.Friction = 0.25f;
            Body.Restitution = 0.75f;
            Body.AngularDamping = 0.075f;

            scene.Input.OnKeyDown += new KeyboardEvent(onKeyDown);
            scene.Input.OnMiddleClickDown += new MouseEvent(onMiddleClickDown);
            Body.UserData = this;
        }

        public override void Update(GameTime gameTime) // Idea: make metroids hover when they near the ground.
        {
            if (sinActive)
            {
                xyMotion();

                //This needs to use Vector2.Length instead of this convoluted mess.
                if (Body.Position.Y > sinCenter.Y + distanceLimit || Body.Position.Y < sinCenter.Y - distanceLimit)
                    sinCenter.Y -= sinCenter.Y - Body.Position.Y;
                if (Body.Position.X > sinCenter.X + distanceLimit || Body.Position.X < sinCenter.X - distanceLimit)
                    sinCenter.X -= sinCenter.X - Body.Position.X;
            }

            base.Update(gameTime);
        }

        private void xyMotion() //Messy and experimental.
        {
            sinForce = Vector2.Zero;

            //Calculate the force of sin(current distance from the sum of the vertices of a revolution)
            sinForce.X = (float)-Math.Sin((Body.Position.X - sinCenter.X));
            sinForce.Y = (float)-Math.Sin(Body.Position.Y - sinCenter.Y);

            Body.ApplyForce(sinForce * MotionRadius);
        }

        void onKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.D1) && scene.Input.RightClickRelease())
                this.Kill();

            else if (keyboardState.IsKeyDown(Keys.Space)) //Caution: experimental, horribly messy, and convoluted.
            {
                sinActive = !sinActive;

                if (MotionRadius == Vector2.Zero)
                    MotionRadius = unitCircle.RandomSegment(Vector2.One);

                sinCenter = Body.Position;
                sinCenter.X -= (float)Math.Sin(MotionRadius.X);

                Body.ApplyLinearImpulse(new Vector2((float)MotionRadius.X / 2f, (float)MotionRadius.Y));
            }
        }

        void onMiddleClickDown(int selectedItemID, MouseState mouseState)
        {
            Body.Awake = true;
            Body.IgnoreGravity ^= true;
        }
    }
}