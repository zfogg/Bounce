using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce.Items
{
    public abstract class Item
    {
        protected Scene scene;
        protected static Random r = BounceGame.r;
        protected SpriteEffects spriteEffects;
        public Color DrawColor = Color.White;

        public Item(Scene scene)
        {
            this.scene = scene;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
