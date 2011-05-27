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
    public class Obstacle : PhysicalSprite
    {
        public Obstacle(Game game, Vector2 position)
            : base(game)
        {
            if (Texture == null)
                Texture = Game.Content.Load<Texture2D>("obstacle");

            Body = BodyFactory.CreateRectangle(BounceGame.World,
                    ConvertUnits.ToSimUnits(this.Texture.Width),
                    ConvertUnits.ToSimUnits(this.Texture.Height), 1);
            Body.Position = position;
            Body.BodyType = BodyType.Static;
            Body.Mass = 1f;
            Body.Restitution = 0.025f;

            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            game.Components.Add(this);
        }

        public Obstacle(Game game)
            : base(game)
        {
            if (Texture == null)
                Texture = Game.Content.Load<Texture2D>("obstacle");

            Body = BodyFactory.CreateRectangle(BounceGame.World,
                    ConvertUnits.ToSimUnits(this.Texture.Width),
                    ConvertUnits.ToSimUnits(this.Texture.Height), 1);
            Body.BodyType = BodyType.Static;
            Body.Mass = 1f;
            Body.Restitution = 0.025f;
            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public override void Initialize()
        {
            BounceGame.PhysicalSprites.Add(this);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw()
        {
            BounceGame.SpriteBatch.Draw(Texture,
                ConvertUnits.ToDisplayUnits(Body.Position), null, Color.White,
                Body.Rotation, origin, 1f, SpriteEffects.None, 0);
        }
    }
}
