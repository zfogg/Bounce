using System;
using Bounce.Scenes;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bounce
{
    public class BounceGame : Game
    {
        //XNA Framework objects
        private GraphicsDeviceManager graphics;
        public static ContentManager ContentManager;

        //Regular objects
        private Camera2D camera;
        private Vector2 windowSize;
        private SceneStack sceneStack;

        public static Random r = new Random();
        public const float MovementCoEf = 3.00f; //Needs more thought.
        public const float GravityCoEf = 5f;

        public BounceGame()
        {
            graphics = new GraphicsDeviceManager(this);
            windowSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            ContentManager = new ContentManager(this.Services);
            sceneStack = new SceneStack(this);

            Input.OnKeyDown += new KeyboardEvent(OnKeyDown);
        }

        void OnKeyDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.RightControl) && keyboardState.IsKeyDown(Keys.D1))
                sceneStack.Push(new BrickBreaker(sceneStack, camera, GraphicsDevice));
            if (keyboardState.IsKeyDown(Keys.RightControl) && keyboardState.IsKeyDown(Keys.Delete))
                sceneStack.Pop();
            if (keyboardState.IsKeyDown(Keys.RightShift) && keyboardState.IsKeyDown(Keys.Delete))
                sceneStack.PopToHead();
        }

        protected override void Initialize()
        {
            ContentManager.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            Window.Title = "Bounce";
            IsMouseVisible = true;
            graphics.ApplyChanges();

            ItemFactory.WindowSize = windowSize;
            camera = new Camera2D(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sceneStack.Push(new BrickBreaker(sceneStack, camera, GraphicsDevice));
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                Input.Update(Mouse.GetState(), Keyboard.GetState());
            }

            camera.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(new Color(new Vector3((float)Math.Sin(World.Gravity.X), (float)Math.Cos(World.Gravity.Y), (float)Math.Tan(camera.Zoom))));

            base.Draw(gameTime);
        }
    }
}