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
    public class Paddle : PhysicalItem
    {
        FixedPrismaticJoint fixedPrismJoint;

        public Paddle(World world, Vector2 spawnPosition)
            : base(world)
        {
            Texture = BounceGame.ContentManager.Load<Texture2D>("obstacle");

            Body = BodyFactory.CreateRectangle(world,
                    ConvertUnits.ToSimUnits(this.Texture.Width),
                    ConvertUnits.ToSimUnits(this.Texture.Height), 1);
            Body.Position = spawnPosition;
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 4f;
            Body.Restitution = 1.0125f;
            Body.Friction = float.MaxValue;

            fixedPrismJoint = JointFactory.CreateFixedPrismaticJoint(world, Body, Body.Position, new Vector2(1f, 0f));
            fixedPrismJoint.MaxMotorForce = 100.0f; // maximum force in Newtons
            fixedPrismJoint.UpperLimit = 3f;
            fixedPrismJoint.LowerLimit = -3f;
            fixedPrismJoint.LimitEnabled = true;
            fixedPrismJoint.MotorEnabled = true;

            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Body.UserData = this.GetType();
        }

        public Paddle(World world)
            : base(world)
        {
            Texture = BounceGame.ContentManager.Load<Texture2D>("obstacle");

            Body = BodyFactory.CreateRectangle(world,
                    ConvertUnits.ToSimUnits(this.Texture.Width),
                    ConvertUnits.ToSimUnits(this.Texture.Height), 1);

            Body.BodyType = BodyType.Dynamic;
            Body.Restitution = 0f;
            Body.Friction = 0f;

            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public override void Update(GameTime gametime)
        {
            fixedPrismJoint.MotorSpeed = 0f;
            Vector2 orientation = Vector2.Normalize(world.Gravity);

            if (Input.IsNewState)
            {
                if (Input.KeyboardState.IsKeyDown(Keys.Right))
                {
                    if (orientation.Y >= 0)
                        fixedPrismJoint.MotorSpeed = 2f;
                    if (orientation.Y < 0)
                        fixedPrismJoint.MotorSpeed = -2f;
                }
                if (Input.KeyboardState.IsKeyDown(Keys.Left))
                {
                    if (orientation.Y >= 0)
                        fixedPrismJoint.MotorSpeed = -2f;
                    if (orientation.Y < 0)
                        fixedPrismJoint.MotorSpeed = 2f;
                }
            }
        }
    }
}
