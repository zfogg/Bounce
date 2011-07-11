using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Bounce
{
    public static class ItemFactory
    {
        private static List<PhysicalItem> activeList;
        public static List<PhysicalItem> ActiveList
        {
            set { activeList = value; }
        }

        public static Samus CreateSamus(World world)
        {
            Samus s = new Samus(world);
            activeList.Add(s);

            return s;
        }

        public static Obstacle CreateObstacle(World world, Vector2 spawnPosition)
        {
            Obstacle o = new Obstacle(world);
            o.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            activeList.Add(o);
            return o;
        }

        public static List<Obstacle> CreateRandomObstacles(World world, int number)
        {
            List<Obstacle> obstacleList = new List<Obstacle>();
            Random r = new Random();
            Texture2D obstacleTexture = BounceGame.ContentManager.Load<Texture2D>("obstacle");

            for (int i = 0; i < number && i < BounceGame.CreationLimit; i++)
            {
                Vector2 spawnPosition = new Vector2(
                //Randomly determine the spawning location
                    r.Next( //X axis.
                        (obstacleTexture.Width / 2), //Left: spawn fully inside the screen by at least the obstacle's Texture width.
                        (BounceGame.Graphics.PreferredBackBufferWidth - obstacleTexture.Width)), //Right: spawn fully inside the screen by at least the obstacle's Texture width.
                    r.Next( //Y axis.
                        (int)((float)BounceGame.Graphics.PreferredBackBufferHeight * 0.20f), //Top: spawn below x% of the screen's height.
                        (BounceGame.Graphics.PreferredBackBufferHeight - (obstacleTexture.Height * 2))) //Bottom: spawn above the floor by two of obstacle's Texture height.
                    );

                Obstacle o = CreateObstacle(world, spawnPosition);
                obstacleList.Add(o);
            }

            return obstacleList;
        }

        public static void CreateMetroidAtMouse(World world, MouseState mouseState)
        {
            CreateMetroid(world, new Vector2(mouseState.X, mouseState.Y));
        }

        public static Metroid CreateMetroid(World world, Vector2 spawnPosition)
        {
            Metroid m = new Metroid(world);
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            activeList.Add(m);
            return m;
        }

        public static Metroid CreateMetroid(World world, Vector2 spawnPosition, float sinRadius, float cosRadius)
        {
            Metroid m = new Metroid(world, sinRadius, cosRadius);
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            activeList.Add(m);
            return m;
        }

        public static List<Metroid> CreateMetroidsOnObstacles(World world, List<Obstacle> obstacles, int percentchance)
        {
            List<Metroid> metroidList = new List<Metroid>();

            if (obstacles != null)
            {
                Random r = new Random();
                foreach (Obstacle obstacle in obstacles)
                    if (r.Next(1, 101) > percentchance)
                    {
                        Metroid m = CreateMetroid(world, new Vector2(
                        //Calculate a spawnPosition that is a bit above the center of obstacle.
                        ConvertUnits.ToDisplayUnits(obstacle.Body.Position.X),
                        ConvertUnits.ToDisplayUnits(obstacle.Body.Position.Y - ConvertUnits.ToSimUnits(50f))));

                        metroidList.Add(m);
                    }
            }

            return metroidList;
        }

        public static List<Metroid> CreateHorizontalMetroidRow(World world, int numberofmetroids, Vector2 startingposition, int pixelsapart)
        {
            List<Vector2> spawnPositions = VectorStructures.HorizontalRow(numberofmetroids, startingposition, pixelsapart);
            List<Metroid> metroidList = new List<Metroid>();

            UnitCircle unitCircle = new UnitCircle();
            int radiusIndex = 5;
            foreach (Vector2 spawnPosition in spawnPositions)
            {//Possibly change this to a for loop, because it's not currently randomizing sin and cos properly.
                metroidList.Add(CreateMetroid(world,
                    spawnPosition,
                    (float)unitCircle.RadiansList.Values[radiusIndex],
                    0f));
                radiusIndex += 2;
            }

            return metroidList;
        }
    }
}
