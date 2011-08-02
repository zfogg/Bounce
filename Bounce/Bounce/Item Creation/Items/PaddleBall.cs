using System;
using System.Collections.Generic;
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
            Body.Mass = 5f;
            Body.Restitution = 1.0725f;
            Body.Friction = 1f;

            scene.Input.OnKeyDown += new KeyboardEvent(onKeyDown);
        }

        public override void Update(GameTime gametime)
        {
            Body.LinearVelocity = Vector2.Clamp(
                Body.LinearVelocity, -Vector2.One * BounceGame.MovementCoEf * 2f, Vector2.One * BounceGame.MovementCoEf * 2f);

            base.Update(gametime);
        }

        public void FixToPaddle(Vector2 offsetFromPaddle)
        {
            Body.Position = paddle.Body.Position - offsetFromPaddle;

            var distanceJoint = JointFactory.CreateDistanceJoint(scene.World,
                this.Body, paddle.Body, Vector2.Zero, Vector2.Zero);
            distanceJoint.Frequency = 7f;
            distanceJoint.DampingRatio = 1.25f;

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
    }
}