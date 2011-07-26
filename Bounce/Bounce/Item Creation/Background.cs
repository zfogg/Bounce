using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bounce
{
    public class Background
    {
        private Scene scene;
        private Vector2 position;
        public Background(Scene scene, Vector2 position, string textureName)
        {
            this.position = position;
            this.scene = scene;
            backgroundTexture = BounceGame.ContentManager.Load<Texture2D>(textureName);
        }

        private Texture2D backgroundTexture;

        public void Draw(SpriteBatch spriteBatch)
        {
             spriteBatch.Draw(
                 backgroundTexture,
                 position,
                 null,
                 Color.White,
                 0f,
                 Vector2.Zero,
                 2f,
                 SpriteEffects.None,
                 1f);
        }
    }
}
