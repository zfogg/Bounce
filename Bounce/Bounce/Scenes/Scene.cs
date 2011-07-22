using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bounce.Scenes
{
    public abstract class Scene : DrawableGameComponent
    {
        public World World { get; protected set; }
        private Camera2D camera;
        private DebugBounce debugFarseer;
        private SpriteBatch spriteBatch;
        public Dictionary<IndexKey, PhysicalItem> PhysicalItems { get; private set; }
        private List<PhysicalItem> itemsToKill;

        private Background background;
        public const float GravityCoEf = 5f;
        public bool IsTopScene;

        public Scene(Game game, Camera2D camera, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
            : base(game)
        {
            World = new World(BounceGame.GravityCoEf * Vector2.UnitY);
            this.camera = camera;
            this.spriteBatch = spriteBatch;
            debugFarseer = new DebugBounce(World, camera);
            debugFarseer.Initialize(graphicsDevice, BounceGame.ContentManager);

            PhysicalItems = new Dictionary<IndexKey, PhysicalItem>();
            ItemFactory.ActiveDict = PhysicalItems;
            itemsToKill = new List<PhysicalItem>(ItemFactory.CreationLimit);

            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
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
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(
                SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, null,
                camera.GetTransformation(this.GraphicsDevice));

            foreach (PhysicalItem sprite in PhysicalItems.Values)
                sprite.Draw(spriteBatch);

            spriteBatch.End();

            debugFarseer.Draw();
            base.Draw(gameTime);
        }

        public virtual void Kill()
        {
            foreach (PhysicalItem item in PhysicalItems.Values)
                item.Kill();

            PhysicalItems.Clear();
            World.Clear();
            debugFarseer.Kill();
        }
    }
}
