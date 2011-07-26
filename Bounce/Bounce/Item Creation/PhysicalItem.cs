using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;


namespace Bounce
{
    public abstract class PhysicalItem
    {
        protected World world;
        protected PhysicalScene scene;
        protected static Random r = new Random();
        public Body Body { get; protected set; }
        public bool IsAlive { get; protected set; }
        public Texture2D Texture;
        protected SpriteEffects spriteEffects;
        protected Color drawColor = Color.White;
        protected Vector2 origin;
        public IndexKey IndexKey;

        public PhysicalItem(PhysicalScene scene)
        {
            this.scene = scene;
            this.IsAlive = true;

            scene.World.ContactManager.OnBroadphaseCollision += OnBroadphaseCollision;
            scene.Input.OnRightClickDown += delegate(int ID, MouseState mouseState)
            {
                if (this.IndexKey == ID)
                    if (scene.Input.KeyboardState.IsKeyDown(Keys.Delete)) Kill();
            };
        }

        public virtual void OnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2) { }

        public virtual void Kill()
        {
            this.IsAlive = false;
            Body.Dispose();
        }

        public virtual void Update(GameTime gametime)
        {
            if (Body.IsDisposed)
                this.IsAlive = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(
                    Texture,
                    ConvertUnits.ToDisplayUnits(Body.Position),
                    null,
                    drawColor,
                    Body.Rotation,
                    origin,
                    1f,
                    spriteEffects,
                    0.1f);
        }
    }
}
