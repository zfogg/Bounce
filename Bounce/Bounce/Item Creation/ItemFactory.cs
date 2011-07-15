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
        public static Vector2 WindowSize { private get; set; }
        private const int creationLimit = 1000;
        private static List<PhysicalItem> activeList;
        public static List<PhysicalItem> ActiveList { set { activeList = value; } }
        private static PhysicalItem _newItem;
        private static PhysicalItem newItem
        {
            get
            { return _newItem; }
            set
            {
                if (activeList.Count <= creationLimit)
                {
                    activeList.Add(value);
                    _newItem = value;
                }
                else
                {
                    _newItem = null;
                    throw new Exception("Creation limit reached!");
                }
            }
        }

        public static Samus CreateSamus(World world)
        {
            newItem = new Samus(world);
            newItem.Body.Position = new Vector2(
                ConvertUnits.ToSimUnits(WindowSize.X * 0.20f),
                ConvertUnits.ToSimUnits(WindowSize.Y - (newItem.Texture.Height / 2)));

            return (Samus)newItem;
        }

        public static Paddle CreatePaddle(World world)
        {
            newItem = new Paddle(world);
            return (Paddle)newItem;
        }

        public static Paddle CreatePaddle(World world, Vector2 spawnPosition)
        {
            newItem = new Paddle(world, ConvertUnits.ToSimUnits(spawnPosition));
            return (Paddle)newItem;
        }

        public static List<Body> CreateFraming(World world, int width)
        {
            List<Body> bodyList = new List<Body>();

            float x = WindowSize.X;
            float y = WindowSize.Y;
            for (float i = 0f; i <= x; i += x)
            {
                for (float j = 0f; j <= y; j += y)
                {
                    Body newBody = BodyFactory.CreateRectangle(world,
                        ConvertUnits.ToSimUnits(i == 0 ? x : width),
                        ConvertUnits.ToSimUnits(i == 0 ? width : y),
                        1f);

                    Vector2 position = new Vector2(
                        i == 0 ? x / 2 : (i + (j == 0 ? width / 2 : -i + (-width / 2))),
                        j == 0 ? (i == 0 ? -width / 2 : y / 2) : (i == 0 ? j + width / 2 : j / 2));

                    newBody.Position = ConvertUnits.ToSimUnits(position);
                    newBody.BodyType = BodyType.Static;
                    bodyList.Add(newBody);
                }
            }

            return bodyList;
        }



        public static Obstacle CreateObstacle(World world, Vector2 spawnPosition)
        {
            newItem = new Obstacle(world);
            newItem.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return (Obstacle)newItem;
        }

        public static List<Obstacle> CreateRandomObstacles(World world, int number)
        {
            List<Obstacle> obstacleList = new List<Obstacle>();
            Random r = new Random();
            Texture2D obstacleTexture = BounceGame.ContentManager.Load<Texture2D>("obstacle");

            for (int i = 0; i < number; i++)
            {
                Vector2 spawnPosition = new Vector2(
                //Randomly determine the spawning location
                    r.Next( //X axis.
                        (obstacleTexture.Width / 2), //Left: spawn fully inside the screen by at least the obstacle's Texture width.
                        ((int)WindowSize.X - obstacleTexture.Width)), //Right: spawn fully inside the screen by at least the obstacle's Texture width.
                    r.Next( //Y axis.
                        (int)((float)WindowSize.X * 0.20f), //Top: spawn below x% of the screen's height.
                        ((int)WindowSize.Y - (obstacleTexture.Height * 2))) //Bottom: spawn above the floor by two of obstacle's Texture height.
                    );

                newItem = CreateObstacle(world, spawnPosition);
                obstacleList.Add((Obstacle)newItem);
            }

            return obstacleList;
        }

        public static Metroid CreateMetroid(World world, Vector2 spawnPosition)
        {
            newItem = new Metroid(world);
            newItem.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return (Metroid)newItem;
        }

        public static Metroid CreateMetroid(World world, Vector2 spawnPosition, float sinRadius, float cosRadius)
        {
            newItem = new Metroid(world, sinRadius, cosRadius);
            newItem.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return (Metroid)newItem;
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
                        newItem = CreateMetroid(world, new Vector2(
                        //Calculate a spawnPosition that is a bit above the center of obstacle.
                        ConvertUnits.ToDisplayUnits(obstacle.Body.Position.X),
                        ConvertUnits.ToDisplayUnits(obstacle.Body.Position.Y - ConvertUnits.ToSimUnits(50f))));

                        metroidList.Add((Metroid)newItem);
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
                Metroid m = CreateMetroid(world, spawnPosition, (float)unitCircle.RadiansList.Values[radiusIndex], 0f);
                metroidList.Add(m);
                radiusIndex += 2;
            }

            return metroidList;
        }
    }
}
