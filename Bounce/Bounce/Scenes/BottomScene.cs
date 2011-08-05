using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce
{
    class BottomScene : Scene
    {
        private SpriteFont arial;
        private string drawString;

        public override Vector2 SceneSize {
            get { return new Vector2(
                sceneStack.Game.GraphicsDevice.Viewport.Width, sceneStack.Game.GraphicsDevice.Viewport.Height); } }

        public BottomScene(SceneStack sceneStack)
            : base(sceneStack)
        {
            arial = BounceGame.ContentManager.Load<SpriteFont>(@"arial");
        }

        public override void Initialize()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            drawString = "Press enter.";

            spriteBatch.DrawString(
                arial, drawString, SceneSize / 2f, Color.White, 0f,
                Vector2.UnitX * (drawString.Length / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
