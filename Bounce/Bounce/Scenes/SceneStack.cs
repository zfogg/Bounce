﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bounce
{
    public class SceneStack : DrawableGameComponent
    {
        private LinkedList<Scene> scenes;
        private Camera2D camera;
        public int Count { get { return scenes.Count; } }
        public Scene Top { get { return scenes.First.Value; } }
        public LinkedListNode<Scene> TopNode { get { return scenes.First; } }

        private SpriteBatch spriteBatch;

        public SceneStack(Game game)
            :base(game)
        {
            scenes = new LinkedList<Scene>();
            game.Components.Add(this);
        }

        protected override void LoadContent()
        {
            camera = (Camera2D)Game.Services.GetService(typeof(Camera2D));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public void Push(Scene scene)
        {
            if (Count > 0)
                Top.WhenPushedOnto();

            scene.Initialize();
            scenes.AddFirst(scene);
        }

        public void Pop()
        {
            Top.WhenPopped();
            scenes.RemoveFirst();

            if (Count > 0)
                Top.WhenPoppedDownTo();
        }

        public void PopToBottom()
        {
            while (Count > 1)
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
            
            spriteBatch.Begin(
                sortMode: SpriteSortMode.FrontToBack,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.AnisotropicClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                effect: null,
                transformMatrix: camera.GetTransformation());

            _draw(scenes.First, spriteBatch);
            spriteBatch.End();

            _debugDraw(scenes.First);
            base.Draw(gameTime);
        }

        public void _draw(LinkedListNode<Scene> node, SpriteBatch spriteBatch)
        {
            node.Value.Draw(spriteBatch);

            if (!node.Value.BlockDraw)
                _draw(node.Next, spriteBatch);
        }

        public void _debugDraw(LinkedListNode<Scene> node)
        {
            node.Value.DebugDraw();

            if (!node.Value.BlockDraw)
                _debugDraw(node.Next);
        }
    }
}
