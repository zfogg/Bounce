using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Brick : RectangleItem
    {
        public Brick(PhysicalScene scene, Texture2D texture)
            : base(scene, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height))
        {
            Body.BodyType = BodyType.Static;
            Body.Restitution = 1f;
            DrawColor = randomColor();
            this.Texture = texture;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            scene.Input.OnMouseHover += new MouseEvent(onMouseHover);
            Body.UserData = this;
        }

        public override void Update(GameTime gametime)
        {
            if (scene.Input.KeyboardState.IsKeyDown(Keys.D2) && scene.Input.RightClickRelease())
                this.Kill();

            base.Update(gametime);
        }

        void onMouseHover(int selectedBodyID, MouseState mouseState)
        {
            if (selectedBodyID == Body.BodyId)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    DrawColor = Color.Black;
                else if (mouseState.RightButton == ButtonState.Pressed)
                    DrawColor = randomColor();
            }
        }

        Color randomColor()
        {
            return new Color(r.Next(byte.MaxValue), r.Next(byte.MaxValue), r.Next(byte.MaxValue));
        }
    }
}
