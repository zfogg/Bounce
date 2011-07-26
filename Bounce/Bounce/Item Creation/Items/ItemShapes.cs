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
        public RectangleItem(PhysicalScene scene, int width, int height)
            : this(scene, (float)width, (float)height) { }

        public RectangleItem(PhysicalScene scene, float width, float height)
            : base(scene)
        {
            Body = BodyFactory.CreateRectangle(scene.World, width, height, 1f);
            Rectangle = new Rectangle(0, 0, (int)ConvertUnits.ToDisplayUnits(width), (int)ConvertUnits.ToDisplayUnits(height));

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
        public CircleItem(PhysicalScene scene, float radius)
            : base(scene)
        {
            Body = BodyFactory.CreateCircle(scene.World, radius, 1f);

            Body.UserData = this;
        }
    }
}
