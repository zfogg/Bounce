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
        public Paddle(PhysicalScene scene, Texture2D texture)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height))
        {
            this.Texture = texture;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            drawColor = Color.MidnightBlue;

            var unitCircle = new UnitCircle();

            Body.BodyType = BodyType.Kinematic;
            Body.IgnoreGravity = true;
            Body.Restitution = 1.125f;

            scene.Input.OnKeyHoldDown += new KeyboardEvent(onKeyHoldDown);
            Body.UserData = this;
        }

        public override void Update(GameTime gameTime)
        {
            Body.LinearVelocity = new Vector2(
                MathHelper.SmoothStep(Body.LinearVelocity.X, 0f, 0.25f),
                MathHelper.SmoothStep(Body.LinearVelocity.Y, 0f, 0.25f));

            base.Update(gameTime);
        }

        Vector2 movementVelocity;
        void onKeyHoldDown(KeyboardState keyboardState)
        {
            Vector2 orientation = Vector2.Normalize(scene.World.Gravity);
            movementVelocity = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Right))
                movementVelocity.X = orientation.Y * BounceGame.MovementCoEf;
            else if (keyboardState.IsKeyDown(Keys.Left))
                movementVelocity.X = -orientation.Y * BounceGame.MovementCoEf;

            Body.LinearVelocity += movementVelocity;
        }
    }
}