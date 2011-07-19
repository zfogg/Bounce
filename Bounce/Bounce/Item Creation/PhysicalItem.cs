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
        public PhysicalItem(World world)
        {
            this.world = world;
            this.IsAlive = true;

            world.ContactManager.OnBroadphaseCollision += OnBroadphaseCollision;
            Input.OnRightClick += delegate(int ID, MouseState mouseState)
            {
                if (this.IndexKey == ID)
                    if (Input.KeyboardState.IsKeyDown(Keys.Delete)) Kill();
            };
        }

        protected World world;
        protected static Random r = new Random();
        public Body Body;
        public bool IsAlive;
        public Texture2D Texture;
        protected SpriteEffects spriteEffects;
        protected Color drawColor = Color.White;
        protected Vector2 origin;
        public IndexKey IndexKey;
        public MouseEvent MouseEvents;

        public virtual void OnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2) { }
        public virtual void OnRightClick() {}

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
                    0);
        }
    }
}
