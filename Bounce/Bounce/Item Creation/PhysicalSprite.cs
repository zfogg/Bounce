using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;


namespace Bounce
{
    public class PhysicalSprite
    {
        public PhysicalSprite()
        {
            this.IsAlive = true;
            r = new Random();
            BounceGame.PhysicalSprites.Add(this);
        }

        protected Random r;
        public Body Body;
        public bool IsAlive;
        public Texture2D Texture;
        protected SpriteEffects spriteEffects;
        protected Vector2 origin;

        public virtual void Update(GameTime gametime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                ConvertUnits.ToDisplayUnits(Body.Position),
                null,
                Color.White,
                Body.Rotation,
                origin,
                1f,
                spriteEffects,
                0);
        }
    }
}
