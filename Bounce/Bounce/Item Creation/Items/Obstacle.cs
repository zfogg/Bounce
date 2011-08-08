using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Obstacle : RectangleItem
    {
        public Obstacle(PhysicalScene scene, Texture2D texture)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height))
        {
            this.Texture = texture;
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 1f;
            Body.Restitution = 0.1f * restitutionCoEf;
            Body.Friction = 0.1f * frictionCoEf;

            DrawColor = new Color(Vector4.UnitW);
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            scene.Input.OnKeyDown += new KeyboardEvent(onKeyDown);
            scene.Input.OnKeyUp += new KeyboardEvent(onKeyUp);
            scene.Input.OnLeftClickDown += new MouseEvent(onLeftClick);
            scene.Input.OnRightClickDown += new MouseEvent(onRightClick);
            scene.Input.OnMouseWheel += new MouseEvent(onMouseWheel);
            scene.Input.OnMouseHover += new MouseEvent(onMouseHover);
            Body.UserData = this;

            initializeJoints();
        }
        
        FixedRevoluteJoint fRevoluteJoint;
        FixedAngleJoint fAngleJoint;
        private float restitutionCoEf = 1.5f;
        private float frictionCoEf = 2f;

        private void initializeJoints()
        {
            fRevoluteJoint = JointFactory.CreateFixedRevoluteJoint(scene.World, Body, Body.LocalCenter, Body.Position);
            fRevoluteJoint.MaxMotorTorque = 20f;
            fRevoluteJoint.MotorSpeed = 0f;
            fRevoluteJoint.MotorTorque = 10f;
            fRevoluteJoint.MotorEnabled = false;

            fAngleJoint = JointFactory.CreateFixedAngleJoint(scene.World, Body);
            fAngleJoint.TargetAngle = 0f;
            fAngleJoint.MaxImpulse = 5f;
            fAngleJoint.BiasFactor = 2f;
            fAngleJoint.Softness = .72f;
        }

        public override void Update(GameTime gametime)
        {
            updateBodyProperties();

            fRevoluteJoint.MotorSpeed = motorForce;
            //fRevoluteJoint.MotorSpeed = -Body.AngularVelocity;

            base.Update(gametime);
        }

        private Vector3 change3;
        private void updateBodyProperties()
        {
            Body.Restitution = MathHelper.Clamp(Body.Restitution + (change3.X * 0.125f), 0f, restitutionCoEf);
            Body.ApplyTorque(change3.Y * 1.125f);
            Body.Friction = MathHelper.Clamp(Body.Friction + (change3.Z * 0.125f), 0f, frictionCoEf);
            change3 = Vector3.Zero;

            updateColor();
        }

        private void updateColor()
        {
            DrawColor.R = (byte)((Body.Restitution / restitutionCoEf) * byte.MaxValue);

            if (Math.Abs(Body.Revolutions) % 2 < 1)
                DrawColor.G = (byte)((Math.Abs(Body.Revolutions) % 1f) * byte.MaxValue);
            else
                DrawColor.G = (byte)((1.00f - Math.Abs(Body.Revolutions) % 1f) * byte.MaxValue);

            DrawColor.B = (byte)((Body.Friction / frictionCoEf) * byte.MaxValue);
        }

        public void onLeftClick(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                if (scene.Input.KeyboardState.IsKeyDown(Keys.R))
                    change3 += Vector3.UnitX;

                if (scene.Input.KeyboardState.IsKeyDown(Keys.B))
                    change3 += Vector3.UnitZ;
            }
        }

        public void onRightClick(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                if (scene.Input.KeyboardState.IsKeyDown(Keys.R))
                    change3 += -Vector3.UnitX;

                if (scene.Input.KeyboardState.IsKeyDown(Keys.B))
                    change3 += -Vector3.UnitZ;
            }

            if (scene.Input.KeyboardState.IsKeyDown(Keys.D3))
                this.Kill();
        }

        public void onMouseWheel(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                fAngleJoint.TargetAngle += (scene.Input.MouseWheelVelocity() * 0.125f); 
            }
        }

        public void onMouseHover(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                //fRevoluteJoint.MotorSpeed = Body.AngularVelocity * BounceGame.MovementCoEf;
            }
        }

        float motorForce;
        void onKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
                motorForce = 5f;
            if (keyboardState.IsKeyDown(Keys.Down))
                motorForce = -5f;
        }

        void onKeyUp(KeyboardState previousKeyboardState)
        {
            motorForce = 0f;
        }

        public override void Kill()
        {
            base.Kill();
        }
    }
}
