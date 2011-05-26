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
        public DebugViewXNA DebugViewXNA;
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
            DebugViewXNA = new DebugViewXNA(World);
            DebugViewXNA.AppendFlags(DebugViewFlags.Shape);

            KeyboardState = new KeyboardState();
            MouseState = new MouseState();

            framing = new Framing(this);
            obstacles = ObjectCreator.CreateObstacles(r.Next(1, 6));
            Metroids = ObjectCreator.CreateMetroidsOnObstacles(ref obstacles, 25);
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

        public static KeyboardState PreviousKeyboardState;
        public static MouseState PreviousMouseState;
        protected override void Update(GameTime gameTime)
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            HandleInput(gameTime);

            if (MouseCircle != null)
                MouseCircle.Position = new Vector2(MouseState.X, MouseState.Y);

            PreviousMouseState = MouseState;
            PreviousKeyboardState = KeyboardState;
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            

            SpriteBatch.Begin();
            framing.Draw();

            foreach (Metroid m in Metroids)
                m.Draw();

            foreach (Obstacle o in obstacles)
                o.Draw();

            samus.Draw();
            
            SpriteBatch.End();
            DebugDraw();
            base.Draw(gameTime);
        }

        protected void DebugDraw()
        {
            Matrix proj = Matrix.CreateOrthographic(
                Graphics.PreferredBackBufferWidth / 1f / 100.0f,
                -Graphics.PreferredBackBufferHeight / 1f / 100.0f, 0, 1000000);

            Vector3 campos = new Vector3();
            campos.X = (-Graphics.PreferredBackBufferWidth / 2) / 100.0f;
            campos.Y = (Graphics.PreferredBackBufferHeight / 2) / -100.0f;
            campos.Z = 0;
            Matrix tran = Matrix.Identity;
            tran.Translation = campos;
            Matrix view = tran;

            DebugViewXNA.RenderDebugData(ref proj, ref view);
        }

        public void HandleInput(GameTime gameTime)
        {
            if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
            {
                if (ParseInput.IsUniqueKeyPress(Keys.F1))
                {
                    EnableOrDisableFlag(DebugViewFlags.Shape);
                }
                if (ParseInput.IsUniqueKeyPress(Keys.F2))
                {
                    EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                    EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
                }
                if (ParseInput.IsUniqueKeyPress(Keys.F3))
                {
                    EnableOrDisableFlag(DebugViewFlags.Joint);
                }
                if (ParseInput.IsUniqueKeyPress(Keys.F4))
                {
                    EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                    EnableOrDisableFlag(DebugViewFlags.ContactNormals);
                }
                if (ParseInput.IsUniqueKeyPress(Keys.F5))
                {
                    EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
                }
                if (ParseInput.IsUniqueKeyPress(Keys.F6))
                {
                    EnableOrDisableFlag(DebugViewFlags.Controllers);
                }
                if (ParseInput.IsUniqueKeyPress(Keys.F7))
                {
                    EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
                }
                if (ParseInput.IsUniqueKeyPress(Keys.F8))
                {
                    EnableOrDisableFlag(DebugViewFlags.AABB);
                }
            }

            if (MouseState != PreviousMouseState)
            {
                if (ParseInput.LeftMouseButtonReleased())
                    Metroids.Add( ObjectCreator.CreateMetroidAtMouse() );
            }

            //if (ParseInput.IsUniqueMouseLeftClick())
                //MouseCircle = ObjectCreator.CreateMouseCircle();

            //if (ParseInput.LeftMouseButtonReleased())
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
