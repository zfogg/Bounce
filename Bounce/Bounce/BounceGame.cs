using System;
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
        private Input input;

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

            input = new Input();
            input.OnKeyDown += new KeyboardEvent(onKeyDown);
        }

        protected override void Initialize()
        {
            ContentManager.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            camera = new Camera2D(GraphicsDevice, input);
            this.Services.AddService(camera.GetType(), camera);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            sceneStack.Push(new BottomScene(sceneStack));
            sceneStack.Push(new BrickBreakerScene(sceneStack));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
                input.Update(); //This instance of Input only needs to be updated because it has Camera2D controls subscribed to its events.

            Window.Title = string.Format("FPS: {0:f} | Camera2D.Position: {1:s} | Camera2D.Rotation: {2:f} | Camera2D.Zoom: {3:f} | SceneStack Depth: {4:d}",
                                Math.Round(1f / gameTime.ElapsedGameTime.TotalSeconds),
                                (camera.Position - (sceneStack.Top.SceneSize / 2f)).ToString(),
                                Math.Round(camera.Rotation, 2),
                                Math.Round(camera.Zoom, 2),
                                (sceneStack.Count - 1));

            base.Update(gameTime);
        }

        void onKeyDown(KeyboardState keyboardState)
        {
            if (input.KeyPressUnique(Keys.OemMinus) && !(sceneStack.Top is BottomScene))
                sceneStack.Pop();
            else if (input.KeyPressUnique(Keys.OemPlus) && (sceneStack.Top is BottomScene))
                sceneStack.Push(new BrickBreakerScene(sceneStack));
            else if (input.KeyPressUnique(Keys.RightShift) && keyboardState.IsKeyDown(Keys.Delete))
                sceneStack.PopToBottom();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}