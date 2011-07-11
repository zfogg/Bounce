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
    public class Samus : PhysicalItem
    {
        public Samus(World world)
        {
            Texture = BounceGame.ContentManager.Load<Texture2D>("samus");

            Body = BodyFactory.CreateCompoundPolygon(
                world,
                VectorStructures.TextureToBayazitList(Texture),
                1f, true);

            origin = VectorStructures.TextureToVertices(Texture).GetCentroid(); //Need the centroid, not center, for a polygon body shape.
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
        }

        private Vector2 force;
        public override void Update(GameTime gameTime)
        {
            force = Vector2.Zero;

            if (Input.KeyboardState.GetPressedKeys().Length != 0)
            {
                if (Input.KeyboardState.IsKeyDown(Keys.W))
                {
                    force = Vector2.Add(force, -Vector2.UnitY);
                    if (Input.KeyPressUnique(Keys.W))
                        Body.ApplyLinearImpulse(force * BounceGame.MovementCoEf);
                }
                if (Input.KeyboardState.IsKeyDown(Keys.D))
                {
                    force = Vector2.Add(force, Vector2.UnitX);
                }
                if (Input.KeyboardState.IsKeyDown(Keys.S))
                {
                    force = Vector2.Add(force, Vector2.UnitY);
                }
                if (Input.KeyboardState.IsKeyDown(Keys.A))
                {
                    force = Vector2.Add(force, -Vector2.UnitX);
                }

                Vector2.Normalize(force);
                Body.ApplyForce(force * BounceGame.MovementCoEf);

                if (Input.KeyboardState.IsKeyDown(Keys.Right))
                    Body.ApplyTorque(1f * BounceGame.MovementCoEf);
                if (Input.KeyboardState.IsKeyDown(Keys.Left))
                    Body.ApplyTorque(1f * BounceGame.MovementCoEf);
            }
        }
    }
}