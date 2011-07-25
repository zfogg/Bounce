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
    public class Paddle : PhysicalItem
    {
        FixedPrismaticJoint fixedPrismJoint;

        public Paddle(Scene scene, World world, Texture2D texture, Vector2 spawnPosition)
            : base(scene, world)
        {
            this.Texture = texture;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            drawColor = Color.MidnightBlue;

            var unitCircle = new UnitCircle();
            Body = BodyFactory.CreateSolidArc(world,
                1f, //density
                (float)unitCircle.RadiansList[UnitCircle.CircleRadians.Sixth], //radians
                20, //sides
                ConvertUnits.ToSimUnits(texture.Width + (texture.Height)), //radius
                ConvertUnits.ToSimUnits(spawnPosition) + (Vector2.UnitY * 2f), // position
                (float)Math.PI); //angle

            Body.Position = spawnPosition;
            Body.BodyType = BodyType.Dynamic;
            Body.IgnoreGravity = true;
            Body.Mass = 4f;
            Body.Restitution = 1.125f;
            Body.Friction = 1f;

            fixedPrismJoint = JointFactory.CreateFixedPrismaticJoint(
                world, Body, Body.Position, Vector2.UnitX);
            fixedPrismJoint.MaxMotorForce = 20f; //maximum force in Newtons
            fixedPrismJoint.UpperLimit = 3f;
            fixedPrismJoint.LowerLimit = -3f;
            fixedPrismJoint.LimitEnabled = true;
            fixedPrismJoint.MotorEnabled = true;

            base.OnKeyHoldDown += onKeyHoldDown;
            base.OnKeyUp += onKeyUp;

            Body.UserData = this;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        void onKeyHoldDown(KeyboardState keyboardState)
        {
            Vector2 orientation = Vector2.Normalize(world.Gravity);

            if (keyboardState.IsKeyDown(Keys.Right))
                    fixedPrismJoint.MotorSpeed = (orientation.Y * BounceGame.MovementCoEf) * Body.Mass * 5f;
            if (keyboardState.IsKeyDown(Keys.Left))
                fixedPrismJoint.MotorSpeed = (-orientation.Y * BounceGame.MovementCoEf) * Body.Mass * 5f;
        }

        void onKeyUp(KeyboardState keyboardState)
        {
            fixedPrismJoint.MotorSpeed = 0f;
        }
    }
}