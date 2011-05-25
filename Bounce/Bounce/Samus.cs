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
            //textureData = new uint[texture.Width * texture.Height];
            //texture.GetData<uint>(textureData);
            //textureVertices = PolygonTools.CreatePolygon(textureData, texture.Width, true);
            //centroid = -textureVertices.GetCentroid();
            //textureVertices.Translate(ref centroid);
            //textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);
            //var scale = ConvertUnits.ToSimUnits(Vector2.One);
            ////Since it is a concave polygon, we need to partition it into several smaller convex polygons
            //List<Vertices> list = BayazitDecomposer.ConvexPartition(textureVertices);
            //foreach (Vertices vertices in list)
            //{
            //    vertices.Scale(ref scale);

            //    //When we flip the y-axis, the orientation can change.
            //    //We need to remember that FPE works with CCW polygons only.
            //    vertices.ForceCounterClockWise();
            //}
            
            ////Body properties
            //body = BodyFactory.CreateCompoundPolygon(BounceGame.World, list, 1f);
            body = BodyFactory.CreateRectangle(BounceGame.World, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height), 1f);
            body.BodyType = BodyType.Dynamic;
            body.Mass = 1.25f;
            body.Friction = 0.8f;
            body.Restitution = 0.075f;
            body.Inertia = 1f;
            body.Position = new Vector2(
              ConvertUnits.ToSimUnits(BounceGame.Graphics.PreferredBackBufferWidth * 0.20f),
            ConvertUnits.ToSimUnits(ConvertUnits.ToDisplayUnits(Framing.FloorBody.Position.Y) - (Framing.FloorTexture.Height / 2) - (texture.Height / 2))
            );
            // End body properties
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            r = new Random();
            
            game.Components.Add(this);
        }
        private Random r;

        private Body body;
        private Vertices textureVertices;
        Vector2 centroid;
        uint[] textureData;
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
            BounceGame.KeyBoardState = Keyboard.GetState();

            force = Vector2.Zero;
            if (BounceGame.KeyBoardState.IsKeyDown(Keys.W))
            {
                force.Y = -BounceGame.MovementCoEf;
                body.ApplyForce(force);
            }
            if (BounceGame.KeyBoardState.IsKeyDown(Keys.D))
            {
                force.X = BounceGame.MovementCoEf;
                body.ApplyForce(force);
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            if (BounceGame.KeyBoardState.IsKeyDown(Keys.S))
            {
                force.Y = BounceGame.MovementCoEf;
                body.ApplyForce(force);
            }
            if (BounceGame.KeyBoardState.IsKeyDown(Keys.A))
            {
                force.X = -BounceGame.MovementCoEf;
                body.ApplyForce(force);
            }

            //body.ApplyLinearImpulse(force);

            if (BounceGame.KeyBoardState.IsKeyDown(Keys.Right))
                body.ApplyTorque(BounceGame.MovementCoEf * 1.2f);
            if (BounceGame.KeyBoardState.IsKeyDown(Keys.Left))
                body.ApplyTorque(-BounceGame.MovementCoEf * 1.2f);

            base.Update(gameTime);
        }

        public void Draw()
        {
            BounceGame.SpriteBatch.Draw(texture,
                ConvertUnits.ToDisplayUnits(body.Position), null, Color.White,
                body.Rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
