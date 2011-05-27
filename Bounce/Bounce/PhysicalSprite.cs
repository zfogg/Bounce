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
    public class PhysicalSprite : Microsoft.Xna.Framework.GameComponent
    {
        
        public PhysicalSprite(Game game)
            : base(game)
        {
            this.IsAlive = true;
            this.game = game;
            r = new Random();
            game.Components.Add(this);
        }

        //private s Texture2D texture;
        public Texture2D Texture;
        public Body Body;
        public Shape Shape;

        public bool IsAlive;
        protected Vector2 origin;
        protected SpriteEffects spriteEffects;
        protected Game game;
        protected Random r;

        public virtual void Draw()
        {

        }
    }
}
