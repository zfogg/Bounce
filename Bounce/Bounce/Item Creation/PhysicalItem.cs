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
            world.ContactManager.OnBroadphaseCollision += OnBroadphaseCollision;
            this.IsAlive = true;
        }

        protected World world;
        protected static Random r = new Random();
        public Body Body;
        public bool IsAlive;
        public Texture2D Texture;
        protected SpriteEffects spriteEffects;
        protected Vector2 origin;

        public abstract void Update(GameTime gametime);
        public virtual void OnBroadphaseCollision(ref FixtureProxy fp1, ref FixtureProxy fp2) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                ConvertUnits.ToDisplayUnits(Body.Position),
                null,
                Color.White,
                Body.Rotation,
                origin,
                1f,
                spriteEffects,
                0);
        }
    }
}
