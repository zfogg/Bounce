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
    public class Metroid : PhysicalSprite
    {
        public Metroid(Game game)
            : base(game)
        {
            this.IsAlive = true;

            if (Texture == null)
                Texture = Game.Content.Load<Texture2D>("metroid");

            Body = BodyFactory.CreateCircle(BounceGame.World,
                        ConvertUnits.ToSimUnits(this.Texture.Width / 2),
                        ConvertUnits.ToSimUnits(this.Texture.Height / 2), 1); ;
            Body.BodyType = BodyType.Dynamic;
            Body.Mass = 5f;
            Body.Friction = 0.25f;
            Body.Restitution = .35f;
            Body.AngularDamping = 1.75f;

            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }
        public override void Initialize()
        {
            BounceGame.PhysicalSprites.Add(this);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsAlive)
            {
                if (BounceGame.KeyboardState.GetPressedKeys().Length != 0)
                {
                    if (InputHelper.KeyPressUnique(Keys.Space))
                        Body.ApplyForce(new Vector2(
                            r.Next(-100, 101) * Body.Mass,
                            r.Next(-100, 101) * Body.Mass));

                    if (BounceGame.KeyboardState.IsKeyDown(Keys.Right))
                        Body.ApplyTorque(BounceGame.MovementCoEf * Body.Mass * .001f);
                    if (BounceGame.KeyboardState.IsKeyDown(Keys.Left))
                        Body.ApplyTorque(-BounceGame.MovementCoEf * Body.Mass * .001f);
                }

                if (BounceGame.MouseState != BounceGame.PreviousMouseState)
                {
                    if (InputHelper.RickClickRelease())
                        this.IsAlive = false;
                }
            }

            if (!this.IsAlive)
            {
                Body.Awake = true;
                Body.Dispose();
                BounceGame.PhysicalSprites.Remove(this);
            }

            base.Update(gameTime);
        }
        
        public override void Draw()
        {
            BounceGame.SpriteBatch.Draw(Texture,
                ConvertUnits.ToDisplayUnits(Body.Position),
                null, Color.White,
                Body.Rotation,
                origin,
                1f,
                SpriteEffects.None,
                0);
        }
    }
}
