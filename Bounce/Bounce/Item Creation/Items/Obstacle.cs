using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Obstacle : PhysicalItem
    {
        public Obstacle(World world)
        {
            Texture = BounceGame.ContentManager.Load<Texture2D>("obstacle");

            Body = BodyFactory.CreateRectangle(world,
                    ConvertUnits.ToSimUnits(this.Texture.Width),
                    ConvertUnits.ToSimUnits(this.Texture.Height), 1);

            Body.BodyType = BodyType.Static;
            Body.Restitution = 0.150f;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public override void Update(GameTime gametime)
        {
            
        }
    }
}
