using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce
{
    class BottomScene : Scene2D
    {
        private SpriteFont arial;

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
            spriteBatch.DrawString(
                arial, sceneStack.ToString() + " : " + sceneStack.Count.ToString(), Vector2.Zero, Color.White);
        }

        public override void Kill()
        {
            throw new NotImplementedException();
        }
    }
}
