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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Samus : Microsoft.Xna.Framework.GameComponent
    {
        private Texture2D texture;
        private Vector2 origin;
        private SpriteEffects spriteEffects;

        public Samus(Game game)
            : base(game)
        {
            if (texture == null)
            {
                texture = Game.Content.Load<Texture2D>("samus");
            }

            uint[] textureData = new uint[texture.Width * texture.Height];
            texture.GetData<uint>(textureData);

            Vertices textureVertices = PolygonTools.CreatePolygon(textureData, texture.Width);
            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);
            textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);

            List<Vertices> verticesList = BayazitDecomposer.ConvexPartition(textureVertices);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1)) * 1f;
            foreach (Vertices vertices in verticesList)
            {
                vertices.Scale(ref vertScale);
            }

            Body = BodyFactory.CreateCompoundPolygon(BounceGame.World, verticesList, 8f, true);

            //Body = BodyFactory.CreateRectangle(BounceGame.World, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height), 1f);
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 1.25f;
            Body.Friction = 0.8f;
            Body.Restitution = 0.075f;
            Body.Inertia = 1f;
            Body.Position = new Vector2(
              ConvertUnits.ToSimUnits(BounceGame.Graphics.PreferredBackBufferWidth * 0.20f),
            ConvertUnits.ToSimUnits(ConvertUnits.ToDisplayUnits(Framing.FloorBody.Position.Y) - (Framing.FloorTexture.Height / 2) - (texture.Height / 2))
            );
            // End body properties

            origin = new Vector2(texture.Width / 2, texture.Height / 2); //For a rectangle body shape.
            origin = -centroid; //For a polygon body shape.
            r = new Random();
            
            game.Components.Add(this);
        }
        private Random r;

        public Body Body;
        Shape Shape;
        private Vector2 force;

        public override void Initialize()
        {
            
            base.Initialize();
        }

        public void LoadContent()
        {
            
            
        }

        public override void Update(GameTime gameTime)
        {
            BounceGame.KeyboardState = Keyboard.GetState();

            force = Vector2.Zero;

            if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
            {
                //if (InputDevices.IsUniqueKeypress(Keys.W)
                if (BounceGame.KeyboardState.IsKeyDown(Keys.W))
                {
                    force.Y = -BounceGame.MovementCoEf;
                    Body.ApplyForce(force);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.D))
                {
                    force.X = BounceGame.MovementCoEf;
                    Body.ApplyForce(force);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.S))
                {
                    force.Y = BounceGame.MovementCoEf;
                    Body.ApplyForce(force);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.A))
                {
                    force.X = -BounceGame.MovementCoEf;
                    Body.ApplyForce(force);
                }

                //body.ApplyLinearImpulse(force);

                if (BounceGame.KeyboardState.IsKeyDown(Keys.Right))
                    Body.ApplyTorque(BounceGame.MovementCoEf * 5.2f);
                if (BounceGame.KeyboardState.IsKeyDown(Keys.Left))
                    Body.ApplyTorque(-BounceGame.MovementCoEf * 5.2f);
                //if (InputHelper.KeyPressUnique(Keys.Space))
                    //Body.ApplyLinearImpulse(new Vector2(0f, -2f));
            }

            base.Update(gameTime);
        }

        public void Draw()
        {
            BounceGame.SpriteBatch.Draw(texture,
                ConvertUnits.ToDisplayUnits(Body.Position), null, Color.White,
                Body.Rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
