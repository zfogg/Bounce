using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce
{
    public class PhysicalScene : Scene2D
    {
        protected Camera2D camera;
        override public bool BlockDraw { get { return false; } }

        public World World { get; protected set; }
        protected const float gravityCoEf = 5f;
        protected DebugBounce debugFarseer;

        public List<PhysicalItem> PhysicalItems { get; private set; }
        protected List<PhysicalItem> itemsToKill;

        public PhysicalScene(SceneStack sceneStack)
            : base(sceneStack)
        {
            camera = (Camera2D)sceneStack.Game.Services.GetService(typeof(Camera2D));

            World = new World(BounceGame.GravityCoEf * Vector2.UnitY);
            debugFarseer = new DebugBounce(World, camera);
            debugFarseer.Initialize(sceneStack.GraphicsDevice, BounceGame.ContentManager, Input);

            PhysicalItems = new List<PhysicalItem>();
            itemsToKill = new List<PhysicalItem>();
        }

        public override void Initialize() { }

        public override void Update(GameTime gameTime)
        {
            Input.MouseHoverPhysicalItem(World);

            foreach (PhysicalItem item in PhysicalItems)
            {
                if (item.IsAlive)
                    item.Update(gameTime);
                else
                    itemsToKill.Add(item);
            }
            
            foreach (PhysicalItem item in itemsToKill)
                PhysicalItems.Remove(item);

            itemsToKill.RemoveRange(0, itemsToKill.Count);

            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            debugFarseer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            foreach (PhysicalItem sprite in PhysicalItems)
                sprite.Draw(spriteBatch);
        }

        protected void worldGravityRotation(float piRadians)
        {
            World.Gravity.X = (float)Math.Sin(piRadians) * gravityCoEf;
            World.Gravity.Y = (float)Math.Cos(piRadians) * gravityCoEf;
        }

        public override void WhenPushedOnto()
        {
            World.Enabled = false;
            base.WhenPushedOnto();
        }

        public override void WhenPoppedDownTo()
        {
            World.Enabled = true;
            World.ClearForces();

            base.WhenPoppedDownTo();
        }

        public override void WhenPopped()
        {
            Kill();
            base.WhenPopped();
        }

        public override void DebugDraw()
        {
            debugFarseer.Draw();
            base.DebugDraw();
        }

        public override void Kill()
        {
            World.Enabled = false;
            World.Clear();
        }
    }
}
