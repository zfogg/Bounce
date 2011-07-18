using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
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
            drawColor = new Color(Vector3.Zero);
            Body.BodyType = BodyType.Static;
            Body.IgnoreCCD = true;
            Body.Restitution = 0f;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Body.UserData = this;
        }

        Vector3 changeInColor;

        public override void Update(GameTime gametime)
        {
            if (Input.KeyboardState.IsKeyDown(Keys.D3) && Input.RickClickRelease())
                this.IsAlive = false;

            changeInColor = Vector3.Zero;
            base.Update(gametime);
        }

        private void updateColor(Vector3 changeInColor)
        {
            //Body properties math
            Body.Restitution += changeInColor.X * 0.125f;
            Body.Rotation += changeInColor.Y * 0.125f;
            Body.Friction = MathHelper.Clamp(Body.Friction + (changeInColor.Z * 0.5f), 0f, 5f);

            //Color properties math
            changeInColor.X = Body.Restitution / 2f;
            if (Math.Abs(Body.Revolutions) % 2 < 1)
                changeInColor.Y = Math.Abs(Body.Revolutions) % 2f;
            else
                changeInColor.Y = 1.00f - Math.Abs(Body.Revolutions) % 1f;
            changeInColor.Z = Body.Friction / 5f;

            drawColor = new Color(changeInColor);
        }

        public override void OnMouseHover()
        {
            updateColor(changeInColor);
        }

        public override void OnLeftClick()
        {
            if (Input.KeyboardState.IsKeyDown(Keys.R))
                changeInColor += Vector3.UnitX;

            if (Input.KeyboardState.IsKeyDown(Keys.B))
                changeInColor += Vector3.UnitZ;

            updateColor(changeInColor);
        }

        public override void OnRightClick()
        {
            if (Input.KeyboardState.IsKeyDown(Keys.R))
                changeInColor += -Vector3.UnitX;

            if (Input.KeyboardState.IsKeyDown(Keys.B))
                changeInColor += -Vector3.UnitZ;

            updateColor(changeInColor);

            if (Input.KeyboardState.IsKeyDown(Keys.Delete))
                this.Kill();
        }

        public override void OnMouseWheel()
        {
            if (Input.MouseWheelForwards())
                changeInColor += Vector3.UnitY;

            if (Input.MouseWheelReverse())
                changeInColor += -Vector3.UnitY;

            updateColor(changeInColor);
        }
    }
}
