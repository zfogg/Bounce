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
    public class PaddleBall : CircleItem
    {
        private Paddle paddle;

        public PaddleBall(PhysicalScene scene, Paddle paddle, Texture2D texture)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width / 2f))
        {
            this.paddle = paddle;
            this.Texture = texture;
            this.origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Body.BodyType = BodyType.Dynamic;
            Body.IgnoreGravity = true;
            Body.Mass = 2f;
            Body.Restitution = 1.125f;
            Body.Friction = 0f;

            Body.FixtureList[0].OnSeparation += OnCollision;
        }

        void OnCollision(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.Body.UserData == paddle)
            { }
        }

        public override void Update(GameTime gametime)
        {
            Vector2 velocity = new Vector2(Body.LinearVelocity.X, Body.LinearVelocity.Y);
            if (Body.FixtureList.Count != 0)
            {
                if (Math.Abs(Body.LinearVelocity.X) < 0.125f || Body.LinearVelocity.X == 0f)
                {
                    velocity.X = Body.LinearVelocity.X * (float)r.NextDouble();
                    Body.ApplyLinearImpulse(Vector2.UnitX * (float)r.NextDouble() );
                }

                if (Math.Abs(Body.LinearVelocity.Y) < 0.125f || Body.LinearVelocity.Y == 0f)
                {
                    velocity.Y = Body.LinearVelocity.Y * (float)r.NextDouble();
                    Body.ApplyLinearImpulse(Vector2.UnitY * (float)r.NextDouble());
                }
            }

            Body.LinearVelocity = velocity;

            base.Update(gametime);
        }

        public void FixToPaddle(Vector2 offsetFromPaddle)
        {
            this.Body.Position = paddle.Body.Position - offsetFromPaddle;
            JointFactory.CreateWeldJoint(scene.World, this.Body, paddle.Body,
                offsetFromPaddle,
                Vector2.Zero);
        }
    }
}