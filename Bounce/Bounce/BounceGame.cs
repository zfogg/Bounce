using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Dynamics;


namespace Bounce
{
    public class BounceGame : Microsoft.Xna.Framework.Game
    {
        //XNA Framework objects
        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;
        public static KeyboardState KeyboardState, PreviousKeyboardState;
        public static MouseState MouseState, PreviousMouseState;

        //Farseer Physics objects
        public static World World;
        public DebugBounce DebugFarseer;

        //Regular objects
        public ObjectCreator ObjectCreator;
        private Camera2D camera;
        private List<Obstacle> obstacles;
        public static List<PhysicalSprite> PhysicalSprites;
        private Framing framing;
        private Samus samus;
        List<Metroid> metroids;
        private Random r;
        public static float MovementCoEf = 4.00f; //Needs more thought.
        public static int CreationLimit = 1000; //Needs more thought.

        public BounceGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            KeyboardState = new KeyboardState();
            MouseState = new MouseState();

            World = new World(Vector2.UnitY * 5f);
            DebugFarseer = new DebugBounce(this);

            ObjectCreator = new ObjectCreator(this);
            PhysicalSprites = new List<PhysicalSprite>();
            r = new Random();
        }

        protected override void Initialize()
        {
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 480;
            Window.Title = "Project Bounce";
            IsMouseVisible = true;
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            Services.AddService(typeof(Game), this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera2D(GraphicsDevice);

            framing = new Framing(this);
            samus = new Samus(this);
            obstacles = ObjectCreator.CreateObstacles(r.Next(0, 6));
            ObjectCreator.CreateMetroidsOnObstacles(ref obstacles, 25);
            //metroids = ObjectCreator.CreateHorizontalMetroidRow(5, new Vector2(50, 189), 135);
        }

        protected override void UnloadContent()
        {

        }

        private void handleInput() //This should be refactored to somewhere other than the game loop class.
        {
            if (MouseState != PreviousMouseState)
            {
                if (InputHelper.LeftClickRelease())
                    ObjectCreator.CreateMetroidAtMouse();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            PreviousMouseState = MouseState;
            PreviousKeyboardState = KeyboardState;

            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            handleInput();

            camera.Step(MovementCoEf);
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, null,
                camera.GetTransformation(GraphicsDevice));

            framing.Draw();
            foreach (PhysicalSprite sprite in PhysicalSprites)
                sprite.Draw();
            
            SpriteBatch.End();
            DebugFarseer.Draw();
            base.Draw(gameTime);
        }
    }
}
