using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce.Scenes
{
    class BottomScene : Scene
    {
        private SpriteFont arial;

        public BottomScene(SceneStack sceneStack)
            : base(sceneStack)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            arial = BounceGame.ContentManager.Load<SpriteFont>(@"arial");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                arial, "BottomScene: Don't pop() this.", Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public override void Kill()
        {
            throw new NotImplementedException();
        }
    }
}
