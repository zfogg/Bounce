using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


namespace Bounce
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Samus : PhysicalSprite
    {
        public Samus(Game game)
            : base(game)
        {
            if (Texture == null)
                Texture = Game.Content.Load<Texture2D>("samus");

            uint[] TextureData = new uint[Texture.Width * Texture.Height];
            Texture.GetData<uint>(TextureData);

            Vertices TextureVertices = PolygonTools.CreatePolygon(TextureData, Texture.Width);
            Vector2 centroid = -TextureVertices.GetCentroid();
            TextureVertices.Translate(ref centroid);
            TextureVertices = SimplifyTools.ReduceByDistance(TextureVertices, 4f);

            List<Vertices> verticesList = BayazitDecomposer.ConvexPartition(TextureVertices);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
            foreach (Vertices vertices in verticesList)
                vertices.Scale(ref vertScale);

            Body = BodyFactory.CreateCompoundPolygon(BounceGame.World, verticesList, 1f, true);

            Body.Position = new Vector2(
                ConvertUnits.ToSimUnits(BounceGame.Graphics.PreferredBackBufferWidth * 0.20f),
                ConvertUnits.ToSimUnits(ConvertUnits.ToDisplayUnits(Framing.FloorBody.Position.Y) - (Framing.FloorTexture.Height / 2) - (Texture.Height / 2))
            );
            Body.BodyType = BodyType.Dynamic;
            Body.FixedRotation = true; //This causes the mass to reset.
            Body.Mass = 1.75f;
            Body.Friction = .475f;
            Body.Restitution = 0.025f;
            Body.AngularDamping = 0.50f;

            origin = new Vector2(Texture.Width / 2, Texture.Height / 2); //For a rectangle body shape.
            origin = -centroid; //For a polygon body shape.
        }

        public override void Initialize()
        {
            BounceGame.PhysicalSprites.Add(this);
            base.Initialize();
        }

        private Vector2 force;
        public override void Update(GameTime gameTime)
        {
            BounceGame.KeyboardState = Keyboard.GetState();

            force = Vector2.Zero;

            if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
            {
                //if (InputDevices.IsUniqueKeypress(Keys.W)
                if (BounceGame.KeyboardState.IsKeyDown(Keys.W))
                {
                    force = Vector2.Add(force, -Vector2.UnitY);
                    //Body.ApplyForce(force * BounceGame.MovementCoEf);

                    if (InputHelper.KeyPressUnique(Keys.W))
                        Body.ApplyLinearImpulse(force * BounceGame.MovementCoEf);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.D))
                {
                    force = Vector2.Add(force, Vector2.UnitX);
                    //Body.ApplyForce(force * BounceGame.MovementCoEf);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.S))
                {
                    force = Vector2.Add(force, Vector2.UnitY);
                    //Body.ApplyForce(force * BounceGame.MovementCoEf);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.A))
                {
                    force = Vector2.Add(force, -Vector2.UnitX);
                    //Body.ApplyForce(force * BounceGame.MovementCoEf);
                }

                Vector2.Normalize(force);
                Body.ApplyForce(force * BounceGame.MovementCoEf);

                //body.ApplyLinearImpulse(force);

                if (BounceGame.KeyboardState.IsKeyDown(Keys.Right))
                    Body.ApplyTorque(1f * BounceGame.MovementCoEf);
                if (BounceGame.KeyboardState.IsKeyDown(Keys.Left))
                    Body.ApplyTorque(1f * BounceGame.MovementCoEf);
                //if (InputHelper.KeyPressUnique(Keys.Space))
                    //Body.ApplyLinearImpulse(new Vector2(0f, -2f));
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {
            BounceGame.SpriteBatch.Draw(Texture,
                ConvertUnits.ToDisplayUnits(Body.Position), null, Color.White,
                Body.Rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
