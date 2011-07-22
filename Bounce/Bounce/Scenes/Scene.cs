using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bounce.Scenes
{
    public abstract class Scene
    {
        protected Background background;
        protected SceneStack sceneStack;
        public virtual bool BlockUpdate { get { return true; } }
        public virtual bool BlockDraw { get { return true; } }

        protected Scene(SceneStack sceneStack)
        {
            this.sceneStack = sceneStack;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Kill();
    }
}
