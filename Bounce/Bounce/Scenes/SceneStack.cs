﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bounce
{
    public class SceneStack : DrawableGameComponent
    {
        private LinkedList<Scene2D> scenes;
        private Camera2D camera;
        public int Count { get { return scenes.Count; } }
        public Scene2D Top { get { return scenes.First.Value; } }
        public LinkedListNode<Scene2D> TopNode { get { return scenes.First; } }

        private SpriteBatch spriteBatch;

        public SceneStack(Game game)
            :base(game)
        {
            scenes = new LinkedList<Scene2D>();
            game.Components.Add(this);
        }

        protected override void LoadContent()
        {
            camera = Game.Services.GetService(typeof(Camera2D)) as Camera2D;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public void Push(Scene2D scene)
        {
            if (Count > 0)
                Top.WhenPushedOnto();

            scenes.AddFirst(scene);
            scene.Initialize();
        }

        public void Pop()
        {
            Top.WhenPopped();
            scenes.RemoveFirst();

            if (Count > 0)
                Top.WhenPoppedDownTo();
        }

        public void PopToHead()
        {
            while (Count > 1)
                Pop();
        }

        public override void Update(GameTime gameTime)
        {
            _update(scenes.First, gameTime);
            base.Update(gameTime);
        }

        public void _update(LinkedListNode<Scene2D> node, GameTime gameTime)
        {
            node.Value.Update(gameTime);

            if (!node.Value.BlockUpdate)
                _update(node.Next, gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                null, null, null, null,
                camera.GetTransformation());

            _draw(scenes.First, spriteBatch);

            spriteBatch.End();

            _debugDraw(scenes.First);
            base.Draw(gameTime);
        }

        public void _draw(LinkedListNode<Scene2D> node, SpriteBatch spriteBatch)
        {
            node.Value.Draw(spriteBatch);

            if (!node.Value.BlockDraw)
                _draw(node.Next, spriteBatch);
        }

        public void _debugDraw(LinkedListNode<Scene2D> node)
        {
            node.Value.DebugDraw();

            if (!node.Value.BlockDraw)
                _debugDraw(node.Next);
        }
    }
}
