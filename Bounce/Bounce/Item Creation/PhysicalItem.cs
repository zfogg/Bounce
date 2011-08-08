using System;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public abstract class PhysicalItem
    {
        protected PhysicalScene scene;
        protected static Random r = new Random();
        public Body Body { get; protected set; }
        public bool IsAlive { get; private set; }
        public Texture2D Texture;
        protected SpriteEffects spriteEffects;
        public Color DrawColor = Color.White;
        protected Vector2 origin;
        public IndexKey IndexKey;

        public PhysicalItem(PhysicalScene scene)
        {
            this.scene = scene;
            this.IsAlive = true;

            scene.World.ContactManager.OnBroadphaseCollision += OnBroadphaseCollision;
            scene.Input.OnMouseHover += (int ID, MouseState mouseState) =>
            {
                if (this.Body.BodyId == ID)
                    if (scene.Input.KeyboardState.IsKeyDown(Keys.Delete) && scene.Input.RightClickUnique())
                        Kill();
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
                    DrawColor,
                    Body.Rotation,
                    origin,
                    1f,
                    spriteEffects,
                    1f);
        }

        public void KillOnTouch<T>() where T : PhysicalItem
        {
            this.Body.OnCollision += (Fixture fixtureA, Fixture fixtureB, Contact contact) =>
            {
                var otherItem = fixtureB.Body.UserData as T;
                if (otherItem != null)
                    otherItem.Kill();

                return true;
            };
        }

        public void KillOnTouch(PhysicalItem itemToKill)
        {
            this.Body.OnCollision += (Fixture fixtureA, Fixture fixtureB, Contact contact) =>
            {
                if (fixtureB.Body.UserData == itemToKill)
                    itemToKill.Kill();

                return true;
            };
        }

        public void KillAfterTouch<T>() where T : PhysicalItem
        {
            this.Body.FixtureList[0].OnSeparation = (Fixture fixtureA, Fixture fixtureB) =>
            {
                var otherItem = fixtureB.Body.UserData as T;
                if (otherItem != null)
                    otherItem.Kill();
            };
        }
    }
}
