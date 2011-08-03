using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bounce
{
    public class Background
    {
        private Scene2D scene;
        private Vector2 position;
        public Background(Scene2D scene, Vector2 position, string textureName)
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
                0.5f,
                SpriteEffects.None,
                1f);
        }
    }
}
