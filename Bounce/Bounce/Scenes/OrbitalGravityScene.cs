using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bounce
{
    class OrbitalGravityScene : PhysicalScene
    {
        public OrbitalGravityScene(SceneStack sceneStack)
            : base(sceneStack)
        {
            background = new Background(Vector2.Zero, "space2");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
