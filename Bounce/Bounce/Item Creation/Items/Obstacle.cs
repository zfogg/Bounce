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
            Body.Mass = 3f;
            Body.Restitution = 0.1f * restitutionCoEf;
            Body.Friction = 0.1f * frictionCoEf;
            Body.AngularDamping = 1f;

            drawColor = new Color(Vector4.UnitW);
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            Input.OnRightClick += OnRightClick;
            Input.OnLeftClick += OnLeftClick;
            Input.OnMouseWheel += OnMouseWheel;
            Input.OnMouseHover += OnMouseHover;
            Input.OnKeyDown += OnKeyDown;
            Body.UserData = this;
        }
        
        FixedRevoluteJoint j;
        FixedFrictionJoint fj;
        private float restitutionCoEf = 1.5f;
        private float frictionCoEf = 2f;

        public void Initialize()
        {
            j = JointFactory.CreateFixedRevoluteJoint(world, Body, Body.LocalCenter, Body.Position);
            j.MaxMotorTorque = 20f;
            j.MotorSpeed = 0f;
            j.MotorTorque = 10f;
            j.MotorEnabled = true;

            fj = JointFactory.CreateFixedFrictionJoint(world, Body, Body.LocalCenter * 100);
            //world.AddJoint(JointFactory.CreateFixedFrictionJoint(world, Body, Body.LocalCenter * 100));
            
        }

        public override void Update(GameTime gametime)
        {
            updateBodyProperties();

            //j.MotorSpeed = -Body.AngularVelocity;
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
                Body.ApplyTorque(Input.MouseWheelVelocity() * BounceGame.MovementCoEf * 20f);
            }
        }

        public void OnMouseHover(int ID, MouseState mouseState)
        {
            if (this.IndexKey == ID)
            {
                j.MotorSpeed = Body.AngularVelocity * BounceGame.MovementCoEf;
            }
            else j.MotorSpeed = 0f;
        }

        void OnKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
                j.MotorSpeed = 5f;
            if (keyboardState.IsKeyDown(Keys.Down))
                j.MotorSpeed = -5f;
        }
    }
}
