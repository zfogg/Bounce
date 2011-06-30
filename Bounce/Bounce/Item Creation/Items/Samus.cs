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
    public class Samus : PhysicalSprite
    {
        public Samus(Game game)
            : base(game)
        {
            if (Texture == null)
                Texture = Game.Content.Load<Texture2D>("samus");

            Body = BodyFactory.CreateCompoundPolygon(
                BounceGame.World,
                VectorStructures.TextureToBayazitList(Texture),
                1f, true);

            origin = VectorStructures.TextureToVertices(Texture).GetCentroid(); //For a polygon body shape.
            Body.Position = new Vector2(
                ConvertUnits.ToSimUnits(BounceGame.Graphics.PreferredBackBufferWidth * 0.20f),
                ConvertUnits.ToSimUnits(ConvertUnits.ToDisplayUnits(Framing.FloorBody.Position.Y) - (Framing.FloorTexture.Height / 2) - (Texture.Height / 2))
            );

            Body.BodyType = BodyType.Dynamic;
            Body.FixedRotation = true; //This causes Body.Mass to reset.
            Body.Mass = 1.75f;
            Body.Friction = .475f;
            Body.Restitution = 0.025f;
            Body.AngularDamping = 0.50f;

            BounceGame.PhysicalSprites.Add(this);
        }

        private Vector2 force;
        public override void Update(GameTime gameTime)
        {
            BounceGame.KeyboardState = Keyboard.GetState();
            force = Vector2.Zero;

            if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
            {
                if (BounceGame.KeyboardState.IsKeyDown(Keys.W))
                {
                    force = Vector2.Add(force, -Vector2.UnitY);
                    if (InputHelper.KeyPressUnique(Keys.W))
                        Body.ApplyLinearImpulse(force * BounceGame.MovementCoEf);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.D))
                {
                    force = Vector2.Add(force, Vector2.UnitX);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.S))
                {
                    force = Vector2.Add(force, Vector2.UnitY);
                }
                if (BounceGame.KeyboardState.IsKeyDown(Keys.A))
                {
                    force = Vector2.Add(force, -Vector2.UnitX);
                }

                Vector2.Normalize(force);
                Body.ApplyForce(force * BounceGame.MovementCoEf);

                if (BounceGame.KeyboardState.IsKeyDown(Keys.Right))
                    Body.ApplyTorque(1f * BounceGame.MovementCoEf);
                if (BounceGame.KeyboardState.IsKeyDown(Keys.Left))
                    Body.ApplyTorque(1f * BounceGame.MovementCoEf);
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
