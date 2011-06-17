using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Obstacle : PhysicalSprite
    {
        public Obstacle(Game game)
            : base(game)
        {
            if (Texture == null)
                Texture = Game.Content.Load<Texture2D>("obstacle");

            Body = BodyFactory.CreateRectangle(BounceGame.World,
                    ConvertUnits.ToSimUnits(this.Texture.Width),
                    ConvertUnits.ToSimUnits(this.Texture.Height), 1);
            Body.BodyType = BodyType.Static;
            Body.Restitution = 0.150f;
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
