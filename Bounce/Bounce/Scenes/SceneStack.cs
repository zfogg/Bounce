﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bounce.Scenes
{
    public class SceneStack : DrawableGameComponent
    {
        private LinkedList<Scene> scenes;
        public int Count { get { return scenes.Count; } }

        private SpriteBatch spriteBatch;
        private Camera2D camera;

        public SceneStack(Game game)
            :base(game)
        {
            scenes = new LinkedList<Scene>();
            Push(new BottomScene(this));
            game.Components.Add(this);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera2D(GraphicsDevice);
            base.LoadContent();
        }

        public void Push(Scene scene)
        {
            scenes.AddFirst(scene);
        }

        public void Pop()
        {
            scenes.RemoveFirst();
        }

        public void PopToHead()
        {
            for (int i = Count; i > 1; i--)
                Pop();
        }

        public override void Update(GameTime gameTime)
        {
            _update(scenes.First, gameTime);
            base.Update(gameTime);
        }

        public void _update(LinkedListNode<Scene> node, GameTime gameTime)
        {
            node.Value.Update(gameTime);

            if (!node.Value.BlockUpdate)
                _update(node.Next, gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _draw(scenes.First, spriteBatch);

            base.Draw(gameTime);
        }

        public void _draw(LinkedListNode<Scene> node, SpriteBatch spriteBatch)
        {
            node.Value.Draw(spriteBatch);

            if (!node.Value.BlockDraw)
                _draw(node.Next, spriteBatch);
        }
    }
}
