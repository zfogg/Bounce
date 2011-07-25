using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public abstract class Scene
    {
        protected Background background;
        protected SceneStack sceneStack;
        public virtual Vector2 SceneSize { get; private set; }
        public bool IsTop { get { return sceneStack.Top == this; } }
        public int SceneDepth { get { return sceneDepthIndex; } }
        private int sceneDepthIndex;
        public virtual bool BlockUpdate { get { return true; } }
        public virtual bool BlockDraw { get { return true; } }

        protected Scene(SceneStack sceneStack)
        {
            this.sceneStack = sceneStack;
        }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Kill();

        public virtual void WhenPushedOnto() { sceneDepthIndex++; }
        public virtual void WhenPoppedDownTo() { sceneDepthIndex--; }
        public virtual void WhenPopped() { }
        public virtual void DebugDraw() { }
    }
}
