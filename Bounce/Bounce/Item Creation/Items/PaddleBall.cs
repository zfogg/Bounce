using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;


namespace Bounce
{
    public class PaddleBall : CircleItem
    {
        private Paddle paddle;
        private Vector2 speedLimit;

        public PaddleBall(PhysicalScene scene, Paddle paddle, Texture2D texture)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width / 2f))
        {
            this.paddle = paddle;
            this.Texture = texture;
            this.origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Body.BodyType = BodyType.Dynamic;
            Body.IgnoreGravity = true;
            Body.Mass = 5f;
            Body.Restitution = 1.0725f;
            Body.Friction = 1f;

            scene.Input.OnKeyDown += new KeyboardEvent(onKeyDown);

            Body.OnCollision += new OnCollisionEventHandler(onCollision);
        }

        public override void Update(GameTime gametime)
        {
            Body.LinearVelocity = Vector2.Clamp(
                Body.LinearVelocity, -Vector2.One * speedLimit, Vector2.One * speedLimit);

            speedLimit = Vector2.Lerp(
                speedLimit,
                Vector2.One * 0.5f,
                0.0025f / MathHelper.Distance(Body.Position.Y, paddle.Body.Position.Y));

            base.Update(gametime);
        }

        public void FixToPaddle(Vector2 offsetFromPaddle)
        {
            Body.Position = paddle.Body.Position - offsetFromPaddle;

            var distanceJoint = JointFactory.CreateDistanceJoint(scene.World,
                this.Body, paddle.Body, Vector2.Zero, Vector2.Zero);
            distanceJoint.Frequency = 10.14159f;
            distanceJoint.DampingRatio = 1f;

            var fPrismJoint = JointFactory.CreateFixedPrismaticJoint(scene.World,
                this.Body, Body.Position, Vector2.UnitX);
            fPrismJoint.LowerLimit = ConvertUnits.ToSimUnits(-scene.SceneSize.X);
            fPrismJoint.UpperLimit = ConvertUnits.ToSimUnits(scene.SceneSize.X);
            fPrismJoint.LimitEnabled = true;
        }

        public void LaunchFromPaddle(float forceCoEf)
        {
            removeJoints(Body.JointList);

            var force = new Vector2(BounceGame.r.Next(-100, 101) / 100f, -1);
            this.Body.ApplyLinearImpulse(force * BounceGame.MovementCoEf * forceCoEf);
            speedLimit = Vector2.One * 5f;
        }

        void removeJoints(JointEdge jointEdge)
        {
            scene.World.RemoveJoint(jointEdge.Joint);

            if (jointEdge.Next != null)
                removeJoints(jointEdge.Next);
        }

        void onKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Space) && Body.JointList != null)
                LaunchFromPaddle(3f);
            else if (keyboardState.IsKeyDown(Keys.Space))
                FixToPaddle(ConvertUnits.ToSimUnits(
                    Vector2.UnitY * this.Texture.Height));
        }

        bool onCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            speedLimit = Vector2.Hermite(speedLimit, Vector2.One + (Vector2.UnitX * 1.35f), Vector2.One * 7f, Vector2.Zero, 0.15f);

            return true;
        }
    }
}