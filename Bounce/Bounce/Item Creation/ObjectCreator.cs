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
        Random r;
        UnitCircle unitCircle;
        private Game game;
        public ObjectCreator(Game game) //to be static, maybe the constructor could grab Game game as a GameComponent.
            : base(game)
        {
            r = new Random();
            this.game = game;
            unitCircle = new UnitCircle();
            game.Components.Add(this);
        }

        public Obstacle CreateObstacle(float spawnPositionX, float spawnPositionY)
        {
            Vector2 position = new Vector2(spawnPositionX, spawnPositionY);
            Obstacle o = CreateObstacle(position);

            return o;
        }

        public Obstacle CreateObstacle(Vector2 spawnPosition)
        {
            Obstacle o = new Obstacle();
            o.Body.Position = spawnPosition;

            return o;
        }

        /// <summary>
        /// Returns a list of obstacles that are placed randomly around the screen.
        /// Obstacles will spawn fully inside the screen, at least 20% above the bottom and below the top.
        /// </summary>
        /// <param name="number">The number of obstacles to create.</param>
        /// <returns></returns>
        public List<Obstacle> CreateObstacles(int number)
        {
            List<Obstacle> obstacleList = new List<Obstacle>();
            Vector2 spawnPosition = new Vector2();

            for (int i = 0; i < number && i < BounceGame.CreationLimit; i++)
            {
                Obstacle o = new Obstacle();
                //Randomly determine the spawning location
                spawnPosition.X = ConvertUnits.ToSimUnits(r.Next( //X axis.
                        (0 + o.Texture.Width), //Left: spawn fully inside the screen by at least the obstacle's Texture width.
                        (BounceGame.Graphics.PreferredBackBufferWidth - o.Texture.Width))); //Right: spawn fully inside the screen by at least the obstacle's Texture width.
                spawnPosition.Y = ConvertUnits.ToSimUnits(r.Next( //Y axis.
                        (int)((float)BounceGame.Graphics.PreferredBackBufferHeight * 0.20f), //Top: spawn below x% of the screen's height.
                        (BounceGame.Graphics.PreferredBackBufferHeight - (o.Texture.Height * 2)))); //Bottom: spawn above the floor by two of obstacle's Texture height.

                o.Body.Position = spawnPosition;
                obstacleList.Add(o);
            }

            return obstacleList;
        }

        public Metroid CreateMetroid(Vector2 spawnPosition, float sinRadius, float cosRadius)
        {
            Metroid m = new Metroid(sinRadius, cosRadius);
            m.Body.Position = spawnPosition;

            return m;
        }

        public Metroid CreateMetroid(float spawnPositionX, float spawnPositionY)
        {
            Vector2 position = new Vector2(spawnPositionX, spawnPositionY);
            Metroid m = CreateMetroid(position);

            return m;
        }

        public Metroid CreateMetroid(Vector2 spawnPosition)
        {
            Metroid m = new Metroid();
            m.Body.Position = spawnPosition;

            return m;
        }

        public List<Metroid> CreateMetroidsOnObstacles(ref List<Obstacle> obstacles, int percentchance)
        {
            List<Metroid> metroidList = new List<Metroid>();
            Vector2 spawnPosition = new Vector2();

            foreach (Obstacle obstacle in obstacles)
                if (r.Next(1, 101) > percentchance)
                {
                    Metroid m = new Metroid();
                    //Calculate a spawnPosition that is a bit above the center of obstacle.
                    spawnPosition.X = obstacle.Body.Position.X;
                    spawnPosition.Y = obstacle.Body.Position.Y - ConvertUnits.ToSimUnits(1.2f * (float)m.Texture.Height);

                    m.Body.Position = spawnPosition;
                    metroidList.Add(m);
                }

            return metroidList;
        }

        public List<Metroid> CreateHorizontalMetroidRow(int numberofmetroids, Vector2 startingposition, int pixelsapart)
        {
            List<Vector2> spawnPositions = VectorStructures.HorizontalRow(numberofmetroids, startingposition, pixelsapart);
            List<Metroid> metroidList = new List<Metroid>();

            int radiusIndex = 5;
            foreach (Vector2 spawnPosition in spawnPositions)
            {
                metroidList.Add(CreateMetroid(ConvertUnits.ToSimUnits(spawnPosition),
                    (float)unitCircle.IndexedRadianDictionary(radiusIndex),
                    0f));
                radiusIndex += 2;
            }

            return metroidList;
        }

        public Metroid CreateMetroidAtMouse()
        {
            Metroid m = new Metroid();
            Vector2 spawnPosition = new Vector2(BounceGame.MouseState.X, BounceGame.MouseState.Y);
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return m;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
