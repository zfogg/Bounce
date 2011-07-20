using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;


namespace Bounce
{
    public class Paddle : RectangleItem
    {
        FixedPrismaticJoint fixedPrismJoint;

        public Paddle(World world, Texture2D texture, Vector2 spawnPosition)
            : base(world, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height))
        {
            this.Texture = texture;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            drawColor = Color.MidnightBlue;

            Body.Position = spawnPosition;
            Body.BodyType = BodyType.Dynamic;
            Body.IgnoreGravity = true;
            Body.Mass = 1f;
            Body.Restitution = 1.0125f;
            Body.Friction = 2.5f;

            fixedPrismJoint = JointFactory.CreateFixedPrismaticJoint(
                world, Body, Body.Position, Vector2.UnitX);
            fixedPrismJoint.MaxMotorForce = 10f; //maximum force in Newtons
            fixedPrismJoint.UpperLimit = 3f;
            fixedPrismJoint.LowerLimit = -3f;
            fixedPrismJoint.LimitEnabled = true;
            fixedPrismJoint.MotorEnabled = true;

            Input.OnKeyHoldDown += OnKeyHoldDown;
            Input.OnKeyUp += OnKeyUp;
            Body.UserData = this;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        void OnKeyHoldDown(KeyboardState keyboardState)
        {
            Vector2 orientation = Vector2.Normalize(world.Gravity);

            if (keyboardState.IsKeyDown(Keys.Right))
                    fixedPrismJoint.MotorSpeed = (orientation.Y * BounceGame.MovementCoEf) * Body.Mass;
            if (keyboardState.IsKeyDown(Keys.Left))
                    fixedPrismJoint.MotorSpeed = (-orientation.Y * BounceGame.MovementCoEf) * Body.Mass;
        }

        void OnKeyUp(KeyboardState keyboardState)
        {
            fixedPrismJoint.MotorSpeed = 0f;
        }
    }
}