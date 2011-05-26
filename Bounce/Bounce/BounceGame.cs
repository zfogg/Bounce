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
using FarseerPhysics.DebugViews;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Controllers;


namespace Bounce
{
    public class BounceGame : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static KeyboardState KeyboardState;
        public static MouseState MouseState;
        public static float MovementCoEf = 2.00f;
        public static int CreationLimit = 1000;

        public BounceGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 480;
            Window.Title = "Project Bounce";
            this.IsMouseVisible = true;
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        public static World World;
        public ObjectCreator ObjectCreator;
        public DebugFarseer DebugFarseer;
        public PrimitiveBatch PrimitiveBatch;

        private Random r;

        private List<Obstacle> obstacles;
        public static List<Metroid> Metroids;
        private Framing framing;
        private Samus samus;

        public Body MouseCircle;
        protected override void Initialize()
        {
            r = new Random();
            ObjectCreator = new ObjectCreator(this);
            World = new World(new Vector2(0, 1.25f));
            DebugFarseer = new DebugFarseer(this);

            KeyboardState = new KeyboardState();
            MouseState = new MouseState();

            framing = new Framing(this);
            obstacles = ObjectCreator.CreateObstacles(r.Next(1, 6));
            Metroids = ObjectCreator.CreateMetroidsOnObstacles(ref obstacles, 25);
            //samus = new Samus(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        public static KeyboardState PreviousKeyboardState;
        public static MouseState PreviousMouseState;
        protected override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            HandleInput(gameTime);

            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            base.Update(gameTime);
            PreviousMouseState = MouseState;
            PreviousKeyboardState = KeyboardState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            DebugFarseer.Draw();

            SpriteBatch.Begin();
            framing.Draw();

            foreach (Metroid m in Metroids)
                m.Draw();

            foreach (Obstacle o in obstacles)
                o.Draw();

            //samus.Draw();
            
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        public void HandleInput(GameTime gameTime)
        {
            if (MouseState != PreviousMouseState)
            {
                if (InputHelper.LeftClickRelease())
                    Metroids.Add( ObjectCreator.CreateMetroidAtMouse() );
            }

            if (InputHelper.LeftClickUnique())
                MouseCircle = ObjectCreator.CreateMouseCircle();
        }
    }
}
