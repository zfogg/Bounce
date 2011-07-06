using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Bounce
{
    public class ItemFactory
    {
        Random r;
        UnitCircle unitCircle;
        public ItemFactory()
        {
            r = new Random();
            unitCircle = new UnitCircle();
        }

        public Obstacle CreateObstacle(Vector2 spawnPosition)
        {
            Obstacle o = new Obstacle();
            o.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return o;
        }

        public Obstacle CreateObstacle(float spawnPositionX, float spawnPositionY)
        {
            Vector2 position = new Vector2(spawnPositionX, spawnPositionY);
            Obstacle o = CreateObstacle(position);

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

        public Metroid CreateMetroid(Vector2 spawnPosition)
        {
            Metroid m = new Metroid();
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return m;
        }

        public Metroid CreateMetroid(float spawnPositionX, float spawnPositionY)
        {
            Vector2 position = new Vector2(spawnPositionX, spawnPositionY);
            Metroid m = CreateMetroid(position);

            return m;
        }

        public Metroid CreateMetroid(Vector2 spawnPosition, float sinRadius, float cosRadius)
        {
            Metroid m = new Metroid(sinRadius, cosRadius);
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return m;
        }

        public Metroid CreateMetroid(float spawnPositionX, float spawnPositionY, float sinRadius, float cosRadius)
        {
            Vector2 position = new Vector2(spawnPositionX, spawnPositionY);
            Metroid m = CreateMetroid(position, sinRadius, cosRadius);

            return m;
        }

        public List<Metroid> CreateMetroidsOnObstacles(ref List<Obstacle> obstacles, int percentchance)
        {
            List<Metroid> metroidList = new List<Metroid>();

            if (obstacles != null)
            {
                foreach (Obstacle obstacle in obstacles)
                    if (r.Next(1, 101) > percentchance)
                    {
                        Metroid m = CreateMetroid(
                        //Calculate a spawnPosition that is a bit above the center of obstacle.
                        ConvertUnits.ToDisplayUnits(obstacle.Body.Position.X),
                        ConvertUnits.ToDisplayUnits(obstacle.Body.Position.Y - ConvertUnits.ToSimUnits(50f)));

                        metroidList.Add(m);
                    }
            }

            return metroidList;
        }

        public List<Metroid> CreateHorizontalMetroidRow(int numberofmetroids, Vector2 startingposition, int pixelsapart)
        {
            List<Vector2> spawnPositions = VectorStructures.HorizontalRow(numberofmetroids, startingposition, pixelsapart);
            List<Metroid> metroidList = new List<Metroid>();

            int radiusIndex = 5;
            foreach (Vector2 spawnPosition in spawnPositions)
            {//Possibly change this to a for loop, because it's not currently randomizing sin and cos properly.
                metroidList.Add(CreateMetroid(
                    spawnPosition,
                    (float)unitCircle.RadiansList.Values[radiusIndex],
                    0f));
                radiusIndex += 2;
            }

            return metroidList;
        }

        public Metroid CreateMetroidAtMouse(Matrix cameraMatrix)
        {
            Metroid m = CreateMetroid(
                BounceGame.MouseState.X,
                BounceGame.MouseState.Y);

            return m;
        }
    }
}
