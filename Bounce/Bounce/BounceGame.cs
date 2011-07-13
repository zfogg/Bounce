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
        private SpriteBatch spriteBatch;
        public static ContentManager ContentManager;

        //Farseer Physics objects
        private World world;
        private DebugBounce debugFarseer;

        //Regular objects
        private Camera2D camera;
        private List<PhysicalItem> physicalSprites;
        private Framing framing;
        private Random r;
        public static float MovementCoEf = 3.00f; //Needs more thought.
        public static int CreationLimit = 1000; //Needs more thought.

        public BounceGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            ContentManager = new ContentManager(this.Services);

            world = new World(Vector2.UnitY * 5f);
            debugFarseer = new DebugBounce(world);

            physicalSprites = new List<PhysicalItem>();
            ItemFactory.ActiveList = physicalSprites;
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

            debugFarseer.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera2D();

            framing = new Framing(world);
            ItemFactory.CreateSamus(world);
            ItemFactory.CreatePaddle(world, new Vector2(Graphics.GraphicsDevice.Viewport.Width / 2, Graphics.GraphicsDevice.Viewport.Height - 25));
            //List<Obstacle> obstacles = ItemFactory.CreateRandomObstacles(world, r.Next(0, 6));
            //ItemFactory.CreateMetroidsOnObstacles(world, obstacles, 50);
            //ItemFactory.CreateHorizontalMetroidRow(world, 5, new Vector2(50, 189), 135);
        }

        private void handleInput() //This should be refactored to somewhere other than the game loop class.
        {
            if (Input.IsNewState)
            {
                if (Input.LeftClickRelease())
                    ItemFactory.CreateMetroid(world, Input.MouseCursorPosition);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
                Input.Update(Mouse.GetState(), Keyboard.GetState());

            handleInput();

            for (int i = 0; i < physicalSprites.Count; i++)
            {//TODO Change this to the way flameshadow@##XNA showed you - http://www.monstersoft.com/wp/?p=500#more-500
                if (physicalSprites[i].IsAlive)
                    physicalSprites[i].Update(gameTime);
                else
                {
                    physicalSprites[i].Body.Dispose();
                    physicalSprites.RemoveAt(i);
                    i--;
                }
            }

            camera.Update();
            world.Gravity.X = (float)Math.Sin(camera.Rotation) * 5f;
            world.Gravity.Y = (float)Math.Cos(camera.Rotation) * 5f;

            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            debugFarseer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, null,
                camera.GetTransformation(this.GraphicsDevice));

            framing.Draw(spriteBatch);

            foreach (PhysicalItem sprite in physicalSprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();
            debugFarseer.Draw(camera, this.GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}
