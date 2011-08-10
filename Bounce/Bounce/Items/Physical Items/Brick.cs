using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;


namespace Bounce.Items
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
            scene.Input.OnRightClickUp += new MouseEvent(onRightClickUp);
            Body.UserData = this;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
        }

        Color randomColor()
        {
            return new Color(r.Next(byte.MaxValue), r.Next(byte.MaxValue), r.Next(byte.MaxValue));
        }

        bool isClicked;
        void onMouseHover(int selectedBodyID, MouseState mouseState)
        {
            if (selectedBodyID == Body.BodyId && !isClicked)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    DrawColor = Color.Black;
                    isClicked = true;
                }
                else if (mouseState.RightButton == ButtonState.Pressed)
                {
                    DrawColor = randomColor();
                    isClicked = true;
                }
            }
            else if (selectedBodyID != Body.BodyId)
                isClicked = false;
        }

        void onRightClickUp(int selectedBodyID, MouseState mouseState)
        {
            if (scene.Input.KeyboardState.IsKeyDown(Keys.D2))
                this.Kill();
        }
    }
}
