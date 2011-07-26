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
        FixedPrismaticJoint fixedPrismJoint;

        public Paddle(PhysicalScene scene, Texture2D texture, Vector2 spawnPosition)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height))
        {
            this.Texture = texture;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            drawColor = Color.MidnightBlue;

            var unitCircle = new UnitCircle();
            //Body = BodyFactory.CreateSolidArc(world,
            //    1f, //density
            //    (float)unitCircle.RadiansList[UnitCircle.CircleRadians.Sixth], //radians
            //    20, //sides
            //    ConvertUnits.ToSimUnits(texture.Width + (texture.Height)), //radius
            //    ConvertUnits.ToSimUnits(spawnPosition) + (Vector2.UnitY * 2f), // position
            //    (float)Math.PI); //angle

            Body.Position = spawnPosition;
            Body.BodyType = BodyType.Dynamic;
            Body.IgnoreGravity = true;
            Body.Mass = 5f;
            Body.Restitution = 1.125f;
            Body.Friction = 1f;

            fixedPrismJoint = JointFactory.CreateFixedPrismaticJoint(
                scene.World, Body, Body.Position, Vector2.UnitX);
            fixedPrismJoint.MaxMotorForce = 25f; //maximum force in Newtons
            fixedPrismJoint.UpperLimit = 3f;
            fixedPrismJoint.LowerLimit = -3f;
            fixedPrismJoint.LimitEnabled = true;
            fixedPrismJoint.MotorEnabled = true;

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
                fixedPrismJoint.MotorSpeed = (orientation.Y * BounceGame.MovementCoEf) * 10f;
            else if (keyboardState.IsKeyDown(Keys.Left))
                fixedPrismJoint.MotorSpeed = (-orientation.Y * BounceGame.MovementCoEf) * 10f;
        }

        void onKeyUp(KeyboardState previousKeyboardState)
        {
            if (previousKeyboardState.IsKeyDown(Keys.Right) || previousKeyboardState.IsKeyDown(Keys.Left))
                fixedPrismJoint.MotorSpeed = 0f;
        }
    }
}