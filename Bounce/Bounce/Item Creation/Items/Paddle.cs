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
            Body.Restitution = 1.0125f;
            Body.Friction = 0f;

            scene.Input.OnKeyHoldDown += new KeyboardEvent(onKeyHoldDown);
            Body.UserData = this;
        }

        public override void Update(GameTime gameTime)
        {
            if (Body.Position.X > ConvertUnits.ToSimUnits(scene.SceneSize.X - (this.Texture.Width / 2)))
            {
                drawColor = Color.Red;
                Body.LinearVelocity = -Vector2.UnitX * 1.5f;
            }
            else if ((Body.Position.X < 0f + ConvertUnits.ToSimUnits((this.Texture.Width / 2))))
            {
                drawColor = Color.Red;
                Body.LinearVelocity = Vector2.UnitX * 1.5f;
            }
            else
            {
                drawColor = Color.MidnightBlue;
                Body.LinearVelocity = new Vector2(
                    MathHelper.SmoothStep(Body.LinearVelocity.X, 0f, 0.33f), 0f);
            }

            base.Update(gameTime);
        }

        Vector2 movementVelocity;
        void onKeyHoldDown(KeyboardState keyboardState)
        {
            Vector2 orientation = Vector2.Normalize(scene.World.Gravity);
            movementVelocity = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Right) && Body.Position.X < ConvertUnits.ToSimUnits(scene.SceneSize.X))
                movementVelocity.X = orientation.Y * BounceGame.MovementCoEf;
            else if (keyboardState.IsKeyDown(Keys.Left) && Body.Position.X > 0f)
                movementVelocity.X = -orientation.Y * BounceGame.MovementCoEf;

            Body.LinearVelocity += movementVelocity;
        }
    }
}