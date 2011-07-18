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

            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Body.UserData = GetType();
        }

        public override void Update(GameTime gameTime)
        {
            fixedPrismJoint.MotorSpeed = 0f;
            Vector2 orientation = Vector2.Normalize(world.Gravity);

            if (Input.IsNewState)
            {
                if (Input.KeyboardState.IsKeyDown(Keys.Right))
                {
                    if (orientation.Y >= 0)
                        fixedPrismJoint.MotorSpeed = BounceGame.MovementCoEf * Body.Mass;
                    if (orientation.Y < 0)
                        fixedPrismJoint.MotorSpeed = -BounceGame.MovementCoEf * Body.Mass;
                }
                if (Input.KeyboardState.IsKeyDown(Keys.Left))
                {
                    if (orientation.Y >= 0)
                        fixedPrismJoint.MotorSpeed = -BounceGame.MovementCoEf * Body.Mass;
                    if (orientation.Y < 0)
                        fixedPrismJoint.MotorSpeed = BounceGame.MovementCoEf * Body.Mass;
                }
            }

            base.Update(gameTime);
        }
    }
}