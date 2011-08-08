using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bounce
{
    public class Background
    {
        private Vector2 position;
        public Background(Vector2 position, string textureName)
        {
            this.position = position;
            backgroundTexture = BounceGame.ContentManager.Load<Texture2D>(textureName);
        }

        private Texture2D backgroundTexture;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: backgroundTexture,
                position: position,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 0.5f,
                effects: SpriteEffects.None,
                layerDepth: 0f);
        }
    }
}
