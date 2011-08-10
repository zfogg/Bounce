using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.RegularExpressions;


namespace Bounce.Scenes
{
    public class TimedTransitionScene : Scene
    {
        SpriteFont font;
        string drawString;
        Vector2 stringPosition;

        Scene sceneToPush;
        double timeToPop;

        public override bool BlockDraw { get { return true; } }

        public TimedTransitionScene(SceneStack sceneStack, Scene newScene, float transitionTime, string stringToDraw)
            : base(sceneStack)
        {
            sceneToPush = newScene;
            timeToPop = transitionTime;
            drawString = stringToDraw;
        }

        public override void Initialize()
        {
            font = BounceGame.ContentManager.Load<SpriteFont>("arial");
            stringPosition = sceneToPush.SceneSize / 2f - (font.MeasureString(drawString.ToString()) / 2f);
        }

        public override void Update(GameTime gameTime)
        {
            timeToPop -= gameTime.ElapsedGameTime.TotalSeconds;
            if (timeToPop <= 0f)
                this.transition();
        }

        void transition()
        {
            sceneStack.Pop(); //Pop myself
            sceneStack.Push(sceneToPush);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                spriteFont: font,
                text: string.Format(drawString + " {0:f}", timeToPop),
                position: stringPosition,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 1f,
                effects: SpriteEffects.None,
                layerDepth: stackDepth * 0.8f);
        }
    }
}
