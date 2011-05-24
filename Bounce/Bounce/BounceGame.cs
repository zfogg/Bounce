using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Controllers;
using FarseerPhysics.SamplesFramework;

namespace Bounce
{
    public class BounceGame : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static KeyboardState KeyBoardState;
        public static float MovementCoEf = 2.00f;
        public static int CreationLimit = 1000;

        public BounceGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 480;
            Window.Title = "Project Bounce";
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);
            ScreenManager.Enabled = true;

            FrameRateCounter frameRateCounter = new FrameRateCounter(ScreenManager);
            frameRateCounter.DrawOrder = 101;
            Components.Add(frameRateCounter);
        }

        public static World World;
        public ObjectCreator ObjectCreator;
        public ScreenManager ScreenManager { get; set; }

        private Random r;
       
        private List<Obstacle> obstacles;
        private List<Metroid> metroids;
        private Framing framing;
        private Samus samus;

        protected override void Initialize()
        {
            r = new Random();
            ObjectCreator = new ObjectCreator(this);
            World = new World(new Vector2(0, 1.25f) );
            KeyBoardState = new KeyboardState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Obstacle.Texture = Content.Load<Texture2D>("obstacle");

            framing = new Framing(this);
            samus = new Samus(this);
            obstacles = ObjectCreator.CreateObstacles(r.Next(1, 6));
            metroids = ObjectCreator.CreateMetroidsOnObstacles(ref obstacles, 25);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            KeyBoardState = Keyboard.GetState();

            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();
            framing.Draw();

            foreach (Metroid m in metroids)
                m.Draw();

            foreach (Obstacle o in obstacles)
                o.Draw();

            samus.Draw();
            
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
