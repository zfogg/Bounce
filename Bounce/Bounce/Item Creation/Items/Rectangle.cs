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
    public class Rectangle : PhysicalItem
    {
        public Rectangle(World world, int width, int height)
            : this(world, (float)width, (float)height)
        {
           
        }

        public Rectangle(World world, float width, float height)
            : base(world)
        {
            Body = BodyFactory.CreateRectangle(world, width, height, 1f);
            Body.UserData = this.GetType();
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            
        }
    }
}
