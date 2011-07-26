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
            Body.Restitution = 1.0125f;
            Body.Friction = 0.25f;

            Body.FixtureList[0].OnSeparation += OnCollision;
        }

        void OnCollision(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB.Body.UserData == paddle)
                Body.ApplyLinearImpulse(new Vector2(
                    (Body.Position.X - paddle.Body.Position.X), Body.LinearVelocity.Y) * Math.Abs(Body.Position.X - paddle.Body.Position.X));
        }

        public override void Update(GameTime gametime)
        {
            if (Body.LinearVelocity != Vector2.Zero)
                Body.ApplyForce(new Vector2(
                    (float)(Math.Sign(Body.LinearVelocity.X) / gametime.ElapsedGameTime.Milliseconds * Math.E),
                    (float)(Math.Sign(Body.LinearVelocity.Y) / gametime.ElapsedGameTime.Milliseconds * Math.E)));

            base.Update(gametime);
        }
    }
}