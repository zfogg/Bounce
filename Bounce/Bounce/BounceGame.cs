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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static ContentManager ContentManager;

        //Farseer Physics objects
        public static World World;
        private DebugBounce debugFarseer;

        //Regular objects
        private Camera2D camera;
        private Vector2 windowSize;
        private Dictionary<IndexKey, PhysicalItem> physicalItems;
        private List<PhysicalItem> itemsToKill;
        private Background background;
        public static Random r = new Random();
        public const float MovementCoEf = 3.00f; //Needs more thought.
        public const float GravityCoEf = 5f;

        public BounceGame()
        {
            graphics = new GraphicsDeviceManager(this);
            windowSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            ContentManager = new ContentManager(this.Services);

            World = new World(Vector2.UnitY * 5f);
            debugFarseer = new DebugBounce(World);

            physicalItems = new Dictionary<IndexKey, PhysicalItem>();
            itemsToKill = new List<PhysicalItem>(ItemFactory.CreationLimit);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            Window.Title = "Bounce";
            IsMouseVisible = true;
            graphics.ApplyChanges();
            ContentManager.RootDirectory = "Content";

            ItemFactory.ActiveDict = physicalItems;
            ItemFactory.WindowSize = windowSize;
            debugFarseer.Initialize(GraphicsDevice, ContentManager);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera2D(GraphicsDevice.Viewport);

            background = new Background(Vector2.Zero);
            ItemStructures.CreateFraming(World, windowSize, 20);
            ItemFactory.CreateSamus(World);
            ItemFactory.CreatePaddle(World);
            //List<Obstacle> obstacles = ItemStructures.CreateRandomlyPositionedObstacles(World, windowSize, r.Next(10));
            //ItemStructures.MetroidsNearItems(World,
            //    obstacles.ConvertAll<PhysicalItem>(x => (PhysicalItem)x),
            //    Vector2.UnitY * 50f, 50);
            //ItemStructures.MetroidRow(world, 5, new Vector2(50, 189), 135);
            //ItemStructures.MetroidColumn(world, 5, new Vector2(windowSize.X / 2, 40), 80);
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.IsActive)
                Input.Update(Mouse.GetState(), Keyboard.GetState());

            handleInput();

            foreach (PhysicalItem item in physicalItems.Values)
            {
                if (item.IsAlive)
                    item.Update(gameTime);
                else
                    itemsToKill.Add(item);
            }

            foreach (PhysicalItem item in itemsToKill)
            {
                item.Kill();
                physicalItems.Remove(item.IndexKey);
            }

            itemsToKill.RemoveRange(0, itemsToKill.Count);

            camera.Update();
            World.Gravity.X = (float)Math.Sin(camera.Rotation) * 5f;
            World.Gravity.Y = (float)Math.Cos(camera.Rotation) * 5f;
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            debugFarseer.Update(gameTime);
            base.Update(gameTime);
        }

        private Fixture grabFixtureAt(Vector2 position)
        {
            return World.TestPoint(position);
        }

        private void handleInput() //This should be refactored to somewhere other than the game loop class.
        {
            
            if (Input.IsNewState)
            {
                if (Input.RightClickUnique())
                {
                    Fixture fixture = World.TestPoint(ConvertUnits.ToSimUnits(Input.MouseCursorVector2));
                    PhysicalItem item = physicalItems[(IndexKey)fixture.Body.BodyId];
                    if (fixture != null)
                        if (fixture.Body.UserData == typeof(Metroid) ||
                            fixture.Body.UserData == typeof(Obstacle) ||
                            fixture.Body.UserData == typeof(Brick))
                            physicalItems[(IndexKey)fixture.Body.BodyId].Kill();
                }

                if (Input.KeyboardState.IsKeyDown(Keys.D1) && Input.LeftClickRelease())
                    ItemFactory.CreateMetroid(World, Input.MouseCursorVector2);

                if (Input.KeyboardState.IsKeyDown(Keys.D2) && Input.LeftClickRelease())
                    ItemFactory.CreateBrick(World, Input.MouseCursorVector2);

                if (Input.KeyboardState.IsKeyDown(Keys.D3) && Input.LeftClickRelease())
                    ItemFactory.CreateObstacle(World, Input.MouseCursorVector2);

                if (Input.KeyPressUnique(Keys.PrintScreen))
                    ItemStructures.MetroidRow(World, 5, Input.MouseCursorVector2, 50);

                if (Input.KeyPressUnique(Keys.Scroll))
                    ItemStructures.MetroidColumn(World, 5, Input.MouseCursorVector2, 50);

                if (Input.KeyPressUnique(Keys.D2) && Input.LeftClickRelease())
                    ItemStructures.BrickRow(World, 5, Input.MouseCursorVector2, 40);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, null,
                camera.GetTransformation(this.GraphicsDevice));

            //background.Draw(spriteBatch);

            foreach (PhysicalItem sprite in physicalItems.Values)
                sprite.Draw(spriteBatch);

            spriteBatch.End();
            debugFarseer.Draw(camera, GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}