using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bounce
{
    public class Background
    {
        private Vector2 position;
        public Background(Vector2 position)
        {
            this.position = position;
            backgroundTexture = BounceGame.ContentManager.Load<Texture2D>("background");
        }

        private Texture2D backgroundTexture;

        public void Draw(SpriteBatch spriteBatch)
        {
             spriteBatch.Draw(backgroundTexture, position, Color.White);
        }
    }
}
