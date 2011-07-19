using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Bounce
{
    public class RectangleItem : PhysicalItem
    {
        public RectangleItem(World world, int width, int height)
            : this(world, (float)width, (float)height) { }

        public RectangleItem(World world, float width, float height)
            : base(world)
        {
            Body = BodyFactory.CreateRectangle(world, width, height, 1f);
            Rectangle = new Rectangle(0, 0, (int)ConvertUnits.ToDisplayUnits(width), (int)ConvertUnits.ToDisplayUnits(height));
            UpdatePosition();

            Body.UserData = this;
        }

        public Rectangle Rectangle;

        public void UpdatePosition()
        {
            Rectangle.X = (int)ConvertUnits.ToDisplayUnits(Body.Position.X) - (this.Rectangle.Width / 2);
            Rectangle.Y = (int)ConvertUnits.ToDisplayUnits(Body.Position.Y) - (this.Rectangle.Height / 2);
        }

        public override void Update(GameTime gametime)
        {
            UpdatePosition();

            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    public class CircleItem : PhysicalItem
    {
        public CircleItem(World world, float radius)
            : base(world)
        {
            Body = BodyFactory.CreateCircle(world, radius, 1f);

            Body.UserData = this;
        }
    }
}
