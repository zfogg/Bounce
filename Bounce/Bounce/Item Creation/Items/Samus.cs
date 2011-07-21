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
            : base(world)
        {
            Texture = BounceGame.ContentManager.Load<Texture2D>("samus");

            Body = BodyFactory.CreateCompoundPolygon(
                world, VectorStructures.TextureToBayazitList(Texture), 1f, true);
            origin = VectorStructures.TextureToVertices(Texture).GetCentroid(); //Need the centroid, not center, for a polygon body shape.

            Body.BodyType = BodyType.Dynamic;
            Body.FixedRotation = true; //This causes Body.Mass to reset.
            Body.Mass = 1.70f;
            Body.Friction = .475f;
            Body.Restitution = 0.025f;
            Body.AngularDamping = 0.50f;

            Input.OnKeyDown += OnKeyPressDown;
            Input.OnKeyHoldDown += OnKeyHoldDown;
            Input.OnKeyUp += OnKeyUp;
            Body.UserData = this;
        }

        private Vector2 force;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void OnKeyPressDown(KeyboardState keyboardState)
        {
            if (Input.KeyPressUnique(Keys.W))
                Body.ApplyLinearImpulse(-Vector2.UnitY * BounceGame.MovementCoEf);
        }

        public void OnKeyHoldDown(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W))
                force = -Vector2.UnitY;
            if (keyboardState.IsKeyDown(Keys.D))
                force = Vector2.UnitX;
            if (keyboardState.IsKeyDown(Keys.S))
                force = Vector2.UnitY;
            if (keyboardState.IsKeyDown(Keys.A))
                force = -Vector2.UnitX;

            Vector2.Normalize(force);
            Body.ApplyForce(force * BounceGame.MovementCoEf);
        }

        void OnKeyUp(KeyboardState keyboardState)
        {
            force = Vector2.Zero;
        }
    }
}