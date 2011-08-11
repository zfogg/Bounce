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
        public Dictionary<Scene, int> SceneDepths;
        public int Count { get { return scenes.Count; } }
        public Scene Top { get { return scenes.First.Value; } }
        public LinkedListNode<Scene> TopNode { get { return scenes.First; } }

        private SpriteBatch spriteBatch;
        private Camera2D camera;

        public SceneStack(Game game)
            :base(game)
        {
            scenes = new LinkedList<Scene>();
            SceneDepths = new Dictionary<Scene, int>();
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

            chartSceneDepths();
        }

        public void Pop()
        {
            Top.WhenPopped();
            scenes.RemoveFirst();

            if (Count > 0)
                Top.WhenPoppedDownTo();

            chartSceneDepths();
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

        void _update(LinkedListNode<Scene> node, GameTime gameTime)
        {
            node.Value.Update(gameTime);

            if (!node.Value.BlockUpdate)
                _update(node.Next, gameTime);
        }
         
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin(
                sortMode: SpriteSortMode.BackToFront,
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

        void _draw(LinkedListNode<Scene> node, SpriteBatch spriteBatch)
        {
            node.Value.Draw(spriteBatch);

            if (!node.Value.BlockDraw)
                _draw(node.Next, spriteBatch);
        }

        void _debugDraw(LinkedListNode<Scene> node)
        {
            node.Value.DebugDraw();

            if (!node.Value.BlockDraw)
                _debugDraw(node.Next);
        }

        void chartSceneDepths()
        {
            cleanSceneDepths();

            int i = 0;
            foreach (Scene scene in scenes.Reverse())
                SceneDepths[scene] = i++;
        }

        void cleanSceneDepths()
        {
            var poppedScenes = SceneDepths.Keys.Except(scenes);
            foreach (Scene scene in poppedScenes.ToList())
                SceneDepths.Remove(scene);
        }
    }
}
