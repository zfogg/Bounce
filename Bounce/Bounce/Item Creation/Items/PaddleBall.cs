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

        public PaddleBall(Scene scene, World world, Paddle paddle, Texture2D texture)
            : base(scene, world, ConvertUnits.ToSimUnits(texture.Width / 2f))
        {
            this.paddle = paddle;
            this.Texture = texture;
            this.origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Body.BodyType = BodyType.Dynamic;
            Body.IgnoreGravity = true;
            Body.Mass = 1f;
            Body.Restitution = 1f;
            Body.Friction = 1f;

            positionOnPaddle();
            base.OnKeyDown += onKeyDown;
        }

        public override void Update(GameTime gametime)
        {
            if (Body.LinearVelocity != Vector2.Zero)
                Body.ApplyForce(new Vector2((float)(Math.Sign(Body.LinearVelocity.X / gametime.ElapsedGameTime.Milliseconds)), (float)(Math.Sign(Body.LinearVelocity.Y / gametime.ElapsedGameTime.Milliseconds))));

            base.Update(gametime);
        }

        private void positionOnPaddle()
        {
            JointFactory.CreateWeldJoint(world,
                this.Body, paddle.Body,
                Vector2.UnitY * ConvertUnits.ToSimUnits(Texture.Height * 2),
                Vector2.Zero);
        }

        void onKeyDown(KeyboardState keyboardState)
        {
            if (scene.IsTop)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if (Body.JointList != null)
                    {
                        world.RemoveJoint(Body.JointList.Joint);

                        var force = new Vector2(r.Next(-100, 101) / 100f, -1);
                        Body.ApplyLinearImpulse(force * BounceGame.MovementCoEf);
                    }
                    else
                    {
                        this.Kill();
                        ItemFactory.CreatePaddleBall(world, paddle);
                    }
                }
            }
        }
    }
}
