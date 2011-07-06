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
        public static ContentManager ContentManager;

        //Farseer Physics objects
        public static World World;
        public DebugBounce DebugFarseer;

        //Regular objects
        private Camera2D camera;
        public ItemFactory ObjectCreator;
        public static List<PhysicalSprite> PhysicalSprites;
        private List<Obstacle> obstacles;
        private List<Metroid> metroids;
        private Framing framing;
        private Samus samus;
        private Random r;
        public static float MovementCoEf = 3.00f; //Needs more thought.
        public static int CreationLimit = 1000; //Needs more thought.

        public BounceGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            KeyboardState = new KeyboardState();
            MouseState = new MouseState();
            ContentManager = new ContentManager(Services);

            World = new World(Vector2.UnitY * 5f);
            DebugFarseer = new DebugBounce(this);

            ObjectCreator = new ItemFactory();
            PhysicalSprites = new List<PhysicalSprite>();
            r = new Random();
        }

        protected override void Initialize()
        {
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 480;
            Window.Title = "Bounce";
            IsMouseVisible = true;
            Graphics.ApplyChanges();
            ContentManager.RootDirectory = "Content";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera2D();

            framing = new Framing(this);
            samus = new Samus();
            //obstacles = ObjectCreator.CreateObstacles(r.Next(0, 6));
            //ObjectCreator.CreateMetroidsOnObstacles(ref obstacles, 25);
            metroids = ObjectCreator.CreateHorizontalMetroidRow(5, new Vector2(50, 189), 135);
        }

        protected override void UnloadContent()
        {

        }

        private void handleInput() //This should be refactored to somewhere other than the game loop class.
        {
            if (MouseState != PreviousMouseState)
            {
                if (InputHelper.LeftClickRelease())
                    ObjectCreator.CreateMetroidAtMouse(camera.GetTransformation(GraphicsDevice));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            PreviousMouseState = MouseState;
            PreviousKeyboardState = KeyboardState;

            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            handleInput();

            for (int i = 0; i < PhysicalSprites.Count; i++)
            {//TODO Change this to the way flameshadow@##XNA showed you - http://www.monstersoft.com/wp/?p=500#more-500
                if (PhysicalSprites[i].IsAlive)
                    PhysicalSprites[i].Update(gameTime);
                else
                {
                    PhysicalSprites[i].Body.Dispose();
                    PhysicalSprites.RemoveAt(i);
                    i--;
                }
            }

            camera.Step();
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(
                SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, null,
                camera.GetTransformation(GraphicsDevice));

            framing.Draw();
            foreach (PhysicalSprite sprite in PhysicalSprites)
                sprite.Draw(SpriteBatch);
            
            SpriteBatch.End();
            DebugFarseer.Draw();
            base.Draw(gameTime);
        }
    }
}
