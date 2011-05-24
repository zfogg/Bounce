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
    public class Metroid : Microsoft.Xna.Framework.GameComponent
    {
        public static Texture2D Texture;
        private Vector2 origin;
        //private SpriteEffects spriteEffects;

        public Metroid(Game game)
            : base(game)
        {
            if (Texture == null)
                Texture = Game.Content.Load<Texture2D>("metroid");

            Body = BodyFactory.CreateCircle(BounceGame.World,
                        ConvertUnits.ToSimUnits(Metroid.Texture.Width / 2),
                        ConvertUnits.ToSimUnits(Metroid.Texture.Height / 2), 1); ;
            Body.BodyType = BodyType.Dynamic;
            //Body.Mass = 0.5f;
            Body.Restitution = 0.7f;

            origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            r = new Random();
            game.Components.Add(this);
        }
        private Random r;

        public Body Body;
        //private Vertices textureVertices;

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public void Draw()
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
