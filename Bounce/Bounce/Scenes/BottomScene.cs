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
        private Vector2 drawPosition;
        private string drawString;

        public override Vector2 SceneSize
        {
            get
            {
                return new Vector2(
                    sceneStack.Game.GraphicsDevice.Viewport.Width, sceneStack.Game.GraphicsDevice.Viewport.Height);
            }
        }

        public BottomScene(SceneStack sceneStack)
            : base(sceneStack)
        {
            arial = BounceGame.ContentManager.Load<SpriteFont>(@"arial");
        }

        public override void Initialize()
        {
            drawString = this.ToString();
            drawPosition = (SceneSize / 2f) - (arial.MeasureString(drawString) / 2f);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                arial,
                drawString,
                drawPosition,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0f);
        }
    }
}
