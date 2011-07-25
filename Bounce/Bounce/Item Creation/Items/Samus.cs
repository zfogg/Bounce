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
        public Samus(Scene scene, World world)
            : base(scene, world)
        {
            Texture = BounceGame.ContentManager.Load<Texture2D>("samus");

            Body = BodyFactory.CreateCompoundPolygon(
                world, VectorStructures.TextureToBayazitList(Texture), 1f, true);
            origin = VectorStructures.TextureToVertices(Texture).GetCentroid(); //Need the centroid, not center, for a polygon body shape.

            Body.BodyType = BodyType.Dynamic;
            Body.FixedRotation = true; //This causes Body.Mass to reset.
            Body.Mass = 4.50f;
            Body.Friction = .475f;
            Body.Restitution = 0.55f;
            Body.AngularDamping = 0.50f;
            Body.LinearDamping = 0.25f;

            base.OnKeyDown += onKeyPressDown;
            base.OnKeyHoldDown += onKeyHoldDown;
            base.OnKeyUp += OnKeyUp;
            Body.UserData = this;
        }

        private Vector2 force;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void onKeyPressDown(KeyboardState keyboardState)
        {
            if (scene.IsTop)
                if (Input.KeyPressUnique(Keys.W))
                    Body.ApplyLinearImpulse((-Vector2.UnitY * (Body.Mass / 2 + Body.LinearVelocity.Y) * BounceGame.MovementCoEf));
        }

        public void onKeyHoldDown(KeyboardState keyboardState)
        {
            if (scene.IsTop)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                    force = -Vector2.UnitY * Body.Mass;
                if (keyboardState.IsKeyDown(Keys.D))
                    force = Vector2.UnitX * Body.Mass;
                if (keyboardState.IsKeyDown(Keys.S))
                    force = Vector2.UnitY * Body.Mass;
                if (keyboardState.IsKeyDown(Keys.A))
                    force = -Vector2.UnitX * Body.Mass;

                Vector2.Normalize(force);
                Body.ApplyForce(force * BounceGame.MovementCoEf);
            }
        }

        void onKeyUp(KeyboardState keyboardState)
        {
            force = Vector2.Zero;
        }
    }
}