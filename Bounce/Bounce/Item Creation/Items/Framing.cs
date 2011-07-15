using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Background
    {
        public Background()
        {
            backgroundTexture = BounceGame.ContentManager.Load<Texture2D>("background");
        }

        private Texture2D backgroundTexture;

        public void Draw(SpriteBatch spriteBatch)
        {
             spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
        }
    }
}
