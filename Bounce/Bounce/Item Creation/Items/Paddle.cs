﻿using System;
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

        private static Vector2 physicalScale = (Vector2.UnitX * 0.64f) + (Vector2.UnitY * .8f);

        public Paddle(PhysicalScene scene, Texture2D texture)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width * physicalScale.X), ConvertUnits.ToSimUnits(texture.Height * physicalScale.Y))
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
            
            if (Body.Position.X > ConvertUnits.ToSimUnits(scene.SceneSize.X - (this.Texture.Width * physicalScale.X / 2))
                || (Body.Position.X < 0f + ConvertUnits.ToSimUnits(this.Texture.Width * physicalScale.X / 2)))
            {
                drawColor = Color.Red;
                Body.LinearVelocity =
                    Vector2.UnitX * BounceGame.MovementCoEf / 2 *
                    -Math.Sign(Body.Position.X - ConvertUnits.ToSimUnits(this.Texture.Width * physicalScale.X / 2));
            }
            else
                drawColor = Color.MidnightBlue;

            Body.LinearVelocity = new Vector2(
                MathHelper.SmoothStep(Body.LinearVelocity.X, 0f, 0.25f), 0f);

            base.Update(gameTime);
        }

        Vector2 movementVelocity;
        void onKeyHoldDown(KeyboardState keyboardState)
        {
            movementVelocity = Vector2.Zero;

            Vector2 orientation = Vector2.Normalize(scene.World.Gravity);

            if (keyboardState.IsKeyDown(Keys.Right))
                movementVelocity.X = orientation.Y * BounceGame.MovementCoEf;
            else if (keyboardState.IsKeyDown(Keys.Left))
                movementVelocity.X = -orientation.Y * BounceGame.MovementCoEf;

            Body.LinearVelocity += movementVelocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                ConvertUnits.ToDisplayUnits(Body.Position),
                null,
                drawColor,
                Body.Rotation,
                origin,
                physicalScale,
                spriteEffects,
                0f);
        }
    }
}