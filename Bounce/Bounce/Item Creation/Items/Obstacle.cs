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
        public Obstacle(World world, Texture2D texture)
            : base(world, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height))
        {
            this.Texture = texture;
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 1f;
            Body.Restitution = 0.1f * restitutionCoEf;
            Body.Friction = 0.1f * frictionCoEf;
            Body.AngularDamping = 0f;

            drawColor = new Color(Vector4.UnitW);
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Input.OnRightClick += OnRightClick;
            Input.OnLeftClick += OnLeftClick;
            Input.OnMouseWheel += OnMouseWheel;
            Input.OnMouseHover += OnMouseHover;
            Input.OnKeyDown += OnKeyDown;
            Input.OnKeyUp += OnKeyUp;
            Body.UserData = this;
        }
        
        FixedRevoluteJoint fRevoluteJoint;
        FixedAngleJoint fAngleJoint;
        private float restitutionCoEf = 1.5f;
        private float frictionCoEf = 2f;

        public void Initialize()
        {
            fRevoluteJoint = JointFactory.CreateFixedRevoluteJoint(world, Body, Body.LocalCenter, Body.Position);
            fRevoluteJoint.MaxMotorTorque = 20f;
            fRevoluteJoint.MotorSpeed = 0f;
            fRevoluteJoint.MotorTorque = 10f;
            fRevoluteJoint.MotorEnabled = false;

            fAngleJoint = JointFactory.CreateFixedAngleJoint(world, Body);
            fAngleJoint.TargetAngle = 0f;
            fAngleJoint.MaxImpulse = 5f;
            fAngleJoint.BiasFactor = 1f;
            fAngleJoint.Softness = .96f;

            //world.AddJoint(JointFactory.CreateFixedFrictionJoint(world, Body, Body.LocalCenter * 100));
            
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
            drawColor.R = (byte)((Body.Restitution / restitutionCoEf) * byte.MaxValue);

            if (Math.Abs(Body.Revolutions) % 2 < 1)
                drawColor.G = (byte)((Math.Abs(Body.Revolutions) % 2f) * byte.MaxValue);
            else
                drawColor.G = (byte)((1.00f - Math.Abs(Body.Revolutions) % 1f) * byte.MaxValue);

            drawColor.B = (byte)((Body.Friction / frictionCoEf) * byte.MaxValue);
        }

        public void OnLeftClick(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                if (Input.KeyboardState.IsKeyDown(Keys.R))
                    change3 += Vector3.UnitX;

                if (Input.KeyboardState.IsKeyDown(Keys.B))
                    change3 += Vector3.UnitZ;
            }
        }

        public void OnRightClick(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                if (Input.KeyboardState.IsKeyDown(Keys.R))
                    change3 += -Vector3.UnitX;

                if (Input.KeyboardState.IsKeyDown(Keys.B))
                    change3 += -Vector3.UnitZ;
            }
        }

        public void OnMouseWheel(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                
            }
        }

        public void OnMouseHover(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                Body.ApplyTorque(Input.MouseWheelVelocity() * BounceGame.MovementCoEf * 20f); 
                //fRevoluteJoint.MotorSpeed = Body.AngularVelocity * BounceGame.MovementCoEf;
            }
        }

        float motorForce;
        void OnKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
                motorForce = 5f;
            if (keyboardState.IsKeyDown(Keys.Down))
                motorForce = -5f;
        }

        void OnKeyUp(KeyboardState keyboardState)
        {
            motorForce = 0f;
        }
    }
}
