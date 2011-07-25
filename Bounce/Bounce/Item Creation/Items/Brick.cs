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
        public Brick(Scene scene, World world, Texture2D texture)
            : base(scene, world, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height))
        {
            Body.BodyType = BodyType.Dynamic;
            drawColor = new Color(r.Next(256), r.Next(256), r.Next(256));
            this.Texture = texture;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Body.UserData = this;
        }

        public void Initialize()
        {
            JointFactory.CreateFixedRevoluteJoint(world, Body, Vector2.Zero, Body.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gametime)
        {
            if (Input.KeyboardState.IsKeyDown(Keys.D2) && Input.RightClickRelease())
                this.IsAlive = false;

            base.Update(gametime);
        }
    }
}
