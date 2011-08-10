using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bounce.Items;


namespace Bounce
{
    public class Background : Item
    {
        private Vector2 position;
        public Background(Scene scene, Vector2 position, string textureName)
            : base(scene)
        {
            this.position = position;
            backgroundTexture = BounceGame.ContentManager.Load<Texture2D>(textureName);
        }

        private Texture2D backgroundTexture;

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
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
                layerDepth: scene.stackDepth);
        }
    }
}
