using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;


namespace Bounce
{
    public class Paddle : RectangleItem
    {
        private FixedPrismaticJoint fPrismJoint;
        public FixedPrismaticJoint FixedPrismJoint
        {
            set
            {
                value.MaxMotorForce = 25f; //maximum force in Newtons
                value.UpperLimit = 3f;
                value.LowerLimit = -3f;
                value.LimitEnabled = true;
                value.MotorEnabled = true;

                fPrismJoint = value;
            }
        }

        public Paddle(PhysicalScene scene, Texture2D texture)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height))
        {
            this.Texture = texture;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            drawColor = Color.MidnightBlue;

            var unitCircle = new UnitCircle();

            Body.BodyType = BodyType.Dynamic;
            Body.IgnoreGravity = true;
            Body.Mass = 5f;
            Body.Restitution = 1.125f;

            scene.Input.OnKeyHoldDown += new KeyboardEvent(onKeyHoldDown);
            scene.Input.OnKeyUp += new KeyboardEvent(onKeyUp);

            Body.UserData = this;
        }

        public override void Update(GameTime gameTime)
        {
            Body.ApplyForce(Vector2.UnitX * -Body.LinearVelocity.X * (float)Math.Sqrt(Body.Mass));

            base.Update(gameTime);
        }

        void onKeyHoldDown(KeyboardState keyboardState)
        {
            Vector2 orientation = Vector2.Normalize(scene.World.Gravity);

            if (keyboardState.IsKeyDown(Keys.Right))
                fPrismJoint.MotorSpeed = (orientation.Y * BounceGame.MovementCoEf) * 10f;
            else if (keyboardState.IsKeyDown(Keys.Left))
                fPrismJoint.MotorSpeed = (-orientation.Y * BounceGame.MovementCoEf) * 10f;
        }

        void onKeyUp(KeyboardState previousKeyboardState)
        {
            if (previousKeyboardState.IsKeyDown(Keys.Right) || previousKeyboardState.IsKeyDown(Keys.Left))
                fPrismJoint.MotorSpeed = 0f;
        }
    }
}