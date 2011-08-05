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
            this.Services.AddService(camera.GetType(), camera); //Needs uncoupling from SceneStack.
                                                                //Maybe each scene should have its own camera, or maybe just a "camera position".
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            sceneStack.Push(new BottomScene(sceneStack));
            sceneStack.Push(new BrickBreaker(sceneStack));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
                input.Update(); //This instance of Input only needs to be updated because it has Camera2D controls subscribed to its events.

            Window.Title = camera.Position.ToString()
                            + " | Rotation: " + camera.Rotation.ToString()
                            + " | Zoom: " + camera.Zoom.ToString()
                            + " | SceneStack Depth: " + (sceneStack.Count - 1).ToString();

            base.Update(gameTime);
        }

        void onKeyDown(KeyboardState keyboardState)
        {
            if (input.KeyPressUnique(Keys.OemMinus) && !(sceneStack.Top is BottomScene))
                sceneStack.Pop();
            else if (input.KeyPressUnique(Keys.OemPlus) && (sceneStack.Top is BottomScene))
                sceneStack.Push(new BrickBreaker(sceneStack));
            else if (input.KeyPressUnique(Keys.RightControl))
            {
                var brickBreaker = new BrickBreaker(sceneStack);
                sceneStack.Push(new TransitionScene(sceneStack, brickBreaker, 5f, "Refreshing: "));
            }
            else if (input.KeyPressUnique(Keys.RightShift) && keyboardState.IsKeyDown(Keys.Delete))
                sceneStack.PopToHead();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}