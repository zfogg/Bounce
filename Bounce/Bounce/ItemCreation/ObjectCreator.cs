using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Bounce
{
    public class ObjectCreator : GameComponent //consider making this into a static class.
    {
        private Game game;
        public ObjectCreator(Game game) //to be static, maybe the constructor could grab Game game as a GameComponent.
            : base(game)
        {
            r = new Random();
            this.game = game;
            unitCircle = new UnitCircle();
            //game.Components.Add(this);
        }
        Random r;
        UnitCircle unitCircle;

        public override void Initialize() //does this method ever even run? Why / why not? If so, what should go into it? $ idea: maybe I should use the constructor to call this.
        {
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
                Obstacle o = new Obstacle(game);
                //The position Vector2 determines the range of the box in which an obstacle can be created.
                position.X = ConvertUnits.ToSimUnits(r.Next( //X axis.
                        (0 + o.Texture.Width), //Left: spawn fully inside the screen by at least the obstacle's Texture width.
                        (BounceGame.Graphics.PreferredBackBufferWidth - o.Texture.Width))); //Right: spawn fully inside the screen by at least the obstacle's Texture width.
                position.Y = ConvertUnits.ToSimUnits(r.Next( //Y axis.
                        (int)((float)BounceGame.Graphics.PreferredBackBufferHeight * 0.20f), //Top: spawn below x% of the screen's height.
                        (BounceGame.Graphics.PreferredBackBufferHeight - (o.Texture.Height * 2)))); //Bottom: spawn above the floor by two of obstacle's Texture height.
                o.Body.Position = position;
                obstacleList.Add(o);
            }

            return obstacleList;
        }

        public Metroid CreateMetroid(Vector2 position, float sinRadius, float cosRadius)
        {
            Metroid m = new Metroid(game, sinRadius, cosRadius);
            m.Body.Position = position;

            return m;
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

        public List<Metroid> CreateHorizontalMetroidRow(int numberofmetroids, Vector2 startingposition, int pixelsapart)
        {
            List<Vector2> positions = Arrangements.HorizontalRow(numberofmetroids, startingposition, pixelsapart);

            List<Metroid> metroidList = new List<Metroid>();
            int i = 5;
            foreach (Vector2 position in positions)
            {
                metroidList.Add(CreateMetroid(ConvertUnits.ToSimUnits(position),
                    (float)unitCircle.IndexedRadianDictionary(i),
                    0f));
                i += 2;
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
