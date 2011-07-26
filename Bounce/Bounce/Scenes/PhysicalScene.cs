using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bounce
{
    public class PhysicalScene : Scene
    {
        public World World { get; protected set; }
        protected const float gravityCoEf = 5f;
        protected DebugBounce debugFarseer;

        override public bool BlockDraw { get { return false; } }

        public Dictionary<IndexKey, PhysicalItem> PhysicalItems { get; private set; }
        protected List<PhysicalItem> itemsToKill;

        public PhysicalScene(SceneStack sceneStack, Camera2D camera)
            : base(sceneStack)
        {
            PhysicalItems = new Dictionary<IndexKey, PhysicalItem>();
            itemsToKill = new List<PhysicalItem>(ItemFactory.CreationLimit);

            World = new World(BounceGame.GravityCoEf * Vector2.UnitY);
            debugFarseer = new DebugBounce(World, camera);
            debugFarseer.Initialize(sceneStack.GraphicsDevice, BounceGame.ContentManager, Input);
        }

        public override void Initialize()
        {
            ItemFactory.ActiveDict = PhysicalItems;
            ItemFactory.ActiveScene = this;
        }

        public override void Update(GameTime gameTime)
        {
            Input.MouseHoverPhysicalItem(World);

            foreach (PhysicalItem item in PhysicalItems.Values)
            {
                if (item.IsAlive)
                    item.Update(gameTime);
                else
                    itemsToKill.Add(item);
            }

            foreach (PhysicalItem item in itemsToKill)
            {
                item.Kill();
                PhysicalItems.Remove(item.IndexKey);
            }

            itemsToKill.RemoveRange(0, itemsToKill.Count);

            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            debugFarseer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            foreach (PhysicalItem sprite in PhysicalItems.Values)
                sprite.Draw(spriteBatch);
        }

        public override void WhenPushedOnto()
        {
            World.Enabled = false;
            base.WhenPushedOnto();
        }

        public override void WhenPoppedDownTo()
        {
            ItemFactory.ActiveDict = this.PhysicalItems;
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
