using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;


namespace Bounce
{
    public abstract class PhysicalSprite
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
        protected Vector2 sinCenter;
        protected float offset;
        protected float radius;

        public abstract void Update(GameTime gametime);
        public abstract void Draw();
        public virtual void Kill()
        {

        }
    }
}
