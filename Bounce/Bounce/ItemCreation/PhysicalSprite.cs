using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;


namespace Bounce
{
    public class PhysicalSprite : Microsoft.Xna.Framework.GameComponent
    {
        
        public PhysicalSprite(Game game)
            : base(game)
        {
            this.IsAlive = true;
            this.game = game;
            r = new Random();
            game.Components.Add(this);
        }

        protected Game game;
        protected Random r;
        public Body Body;
        public bool IsAlive;
        public Texture2D Texture;
        protected SpriteEffects spriteEffects;
        
        protected Vector2 origin;
        protected Vector2 sinCenter;
        protected float offset;
        protected float radius;

        public virtual void Draw()
        {

        }
    }
}
