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
        }

        public static World World;
        public ObjectCreator ObjectCreator;
        public DebugViewXNA DebugViewXNA;
        public PrimitiveBatch PrimitiveBatch;

        private Random r;

        private List<Obstacle> obstacles;
        private List<Metroid> metroids;
        private Framing framing;
        private Samus samus;

        protected override void Initialize()
        {
            r = new Random();
            ObjectCreator = new ObjectCreator(this);
            World = new World(new Vector2(0, 1.25f));
            DebugViewXNA = new DebugViewXNA(World);
            DebugViewXNA.AppendFlags(DebugViewFlags.Shape);
            PrimitiveBatch = new PrimitiveBatch(GraphicsDevice);
            //Components.Add<DebugView>(DebugViewXNA);
            KeyBoardState = new KeyboardState();

            framing = new Framing(this);
            obstacles = ObjectCreator.CreateObstacles(r.Next(1, 6));
            metroids = ObjectCreator.CreateMetroidsOnObstacles(ref obstacles, 25);
            samus = new Samus(this);
            base.Initialize();
        }

        SpriteFont font;
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            DebugViewXNA.LoadContent(GraphicsDevice, Content);
            font = Content.Load<SpriteFont>("font");
        }

        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            KeyBoardState = Keyboard.GetState();
            HandleInput(gameTime);
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            

            SpriteBatch.Begin();
            DebugDraw();
            framing.Draw();

            foreach (Metroid m in metroids)
                m.Draw();

            foreach (Obstacle o in obstacles)
                o.Draw();

            samus.Draw();

            SpriteBatch.End();
            base.Draw(gameTime);
        }

        protected void DebugDraw()
        {
            Matrix transform = Matrix.Identity * Matrix.CreateScale(GraphicsDevice.Viewport.Height / ConvertUnits.ToSimUnits(GraphicsDevice.Viewport.Height))
                                    * Matrix.CreateScale(1,-1,1)
                                    * Matrix.CreateTranslation(GraphicsDevice.Viewport.Width * .5f, GraphicsDevice.Viewport.Height * .5f, 0f);
            

            Matrix IDMatrix = Matrix.Identity;
            DebugViewXNA.RenderDebugData(ref transform, ref IDMatrix);
        }

        public void HandleInput(GameTime gameTime)
        {
            // Control debug view
            
            if (KeyBoardState.IsKeyDown(Keys.F1))
            {
                EnableOrDisableFlag(DebugViewFlags.Shape);
            }
            if (KeyBoardState.IsKeyDown(Keys.F2))
            {
                EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
            }
            if (KeyBoardState.IsKeyDown(Keys.F3))
            {
                EnableOrDisableFlag(DebugViewFlags.Joint);
            }
            if (KeyBoardState.IsKeyDown(Keys.F4))
            {
                EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                EnableOrDisableFlag(DebugViewFlags.ContactNormals);
            }
            if (KeyBoardState.IsKeyDown(Keys.F5))
            {
                EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
            }
            if (KeyBoardState.IsKeyDown(Keys.F6))
            {
                EnableOrDisableFlag(DebugViewFlags.Controllers);
            }
            if (KeyBoardState.IsKeyDown(Keys.F7))
            {
                EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
            }
            if (KeyBoardState.IsKeyDown(Keys.F8))
            {
                EnableOrDisableFlag(DebugViewFlags.AABB);
            }
        }

        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugViewXNA.Flags & flag) == flag)
            {
                DebugViewXNA.RemoveFlags(flag);
            }
            else
            {
                DebugViewXNA.AppendFlags(flag);
            }
        }
    }
}
