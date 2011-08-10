using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce.Items
{
    class TextItem : Item
    {
        SpriteFont arial;
        string drawString;
        Vector2 drawPosition;

        public TextItem(Scene scene)
            : base(scene)
        {
            arial = BounceGame.ContentManager.Load<SpriteFont>("arial");
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                arial,
                drawString,
                drawPosition,
                DrawColor,
                0f,
                Vector2.Zero,
                1f,
                spriteEffects,
                scene.stackDepth * 0.8f);
        }
    }
}
