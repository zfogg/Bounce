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
    public class ObjectCreator : Microsoft.Xna.Framework.GameComponent //$ idea: consider making this into a static class.
    {
        private Game game;
        public ObjectCreator(Game game) //$ idea: to be static, maybe the constructor could grab Game game as a GameComponent.
            : base(game)
        {
            r = new Random();
            this.game = game;
            //game.Components.Add(this);
        }
        Random r;

        public override void Initialize() //$ check: does this method ever even run? Why / why not? If so, what should go into it? $ idea: maybe I should use the constructor to call this.
        {
            CreateObstacles(r.Next(1, 6));
            r = new Random();
            base.Initialize();
        }

        public Obstacle CreateObstacle(float positionX, float positionY)
        {
            Vector2 position = new Vector2(positionX, positionY);
            Obstacle o = CreateObstacle(position);
            return o;
        }

        public Obstacle CreateObstacle(Vector2 position)
        {
            Obstacle o = new Obstacle(game);
            o.Body.Position = position;

            return o;
        }

        public List<Obstacle> CreateObstacles(int number)
        {
            Vector2 position = Vector2.Zero;
            List<Obstacle> obstacleList = new List<Obstacle>();
            for (int i = 0; i < number && i < BounceGame.CreationLimit; i++)
            {
                Obstacle o = CreateObstacle(position);
                //The position Vector2 determines the range of the box in which an obstacle can be created.
                position.X = ConvertUnits.ToSimUnits(r.Next( //X axis.
                        (0 + o.Texture.Width), //Left: spawn fully inside the screen by at least the obstacle's Texture width.
                        (BounceGame.Graphics.PreferredBackBufferWidth - o.Texture.Width))); //Right: spawn fully inside the screen by at least the obstacle's Texture width.
                position.Y = ConvertUnits.ToSimUnits(r.Next( //Y axis.
                        (int)((float)BounceGame.Graphics.PreferredBackBufferHeight * 0.20f), //Top: spawn below x% of the screen's height.
                        (BounceGame.Graphics.PreferredBackBufferHeight - (o.Texture.Height * 2)))); //Bottom: spawn above the floor by two of obstacle's Texture height.
                
                obstacleList.Add(o);
            }

            return obstacleList;
        }

        public Metroid CreateMetroid(float positionX, float positionY)
        {
            Vector2 position = new Vector2(positionX, positionY);
            Metroid m = CreateMetroid(position);

            return m;
        }

        public Metroid CreateMetroid(Vector2 position)
        {
            Metroid m = new Metroid(game);
            m.Body.Position = position;

            return m;
        }

        public List<Metroid> CreateMetroidsOnObstacles(ref List<Obstacle> obstacles, int percentchance)
        {
            List<Metroid> metroidList = new List<Metroid>();
            Vector2 position = Vector2.Zero;
            foreach (Obstacle obstacle in obstacles)
                if (r.Next(1, 101) > percentchance)
                {
                    Metroid m = new Metroid(game);
                    //First, calculate a position that is a bit above the center of Obstacle obstacle.
                    m.Body.Position = new Vector2(
                        obstacle.Body.Position.X,
                        position.Y = ConvertUnits.ToSimUnits(ConvertUnits.ToDisplayUnits(obstacle.Body.Position.Y) - (1.2f * (float)m.Texture.Height)));
                    //Next, create a Metroid at position, and add it to the list.
                    metroidList.Add(m);
                }

            return metroidList;
        }

        public Body CreateMouseCircle()
        {
            Body body = BodyFactory.CreateCircle(BounceGame.World, ConvertUnits.ToSimUnits(10), 0f);
            body.Position = new Vector2(BounceGame.MouseState.X, -BounceGame.MouseState.Y);
            //body.IgnoreGravity = true;
            return body;
        }

        public Metroid CreateMetroidAtMouse()
        {
            Metroid m = new Metroid(game);
            Vector2 position = new Vector2(
                ConvertUnits.ToSimUnits(BounceGame.MouseState.X),
                ConvertUnits.ToSimUnits(BounceGame.MouseState.Y));
            m.Body.Position = position;

            return m;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
