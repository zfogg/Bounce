using System;
using System.Collections;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bounce
{
    public static class ItemFactory
    {
        public static Scene ActiveScene { private get; set; } //This is bad code; needs to go.
        public static Dictionary<IndexKey, PhysicalItem> ActiveDict { private get; set; } //This is bad code; needs to go.

        public const int CreationLimit = 1000;
        public static Vector2 WindowSize { private get; set; }

        private static PhysicalItem _newItem;
        private static PhysicalItem newItem
        {
            get
            { return _newItem; }
            set
            {
                if (ActiveDict.Count <= CreationLimit)
                {
                    value.IndexKey = new IndexKey(value.Body.BodyId);
                    ActiveDict.Add(value.IndexKey, value);
                    _newItem = value;
                }
                else
                {
                    _newItem = null;
                    throw new Exception("Creation limit reached!");
                }
            }
        }

        public static PaddleBall CreatePaddleBall(PhysicalScene scene, Paddle paddle)
        {
            Texture2D texture = BounceGame.ContentManager.Load<Texture2D>("dragonBall");
            newItem = new PaddleBall(scene, paddle, texture);
            newItem.Body.Position = paddle.Body.Position + ConvertUnits.ToSimUnits((-Vector2.UnitY * 50f));
            return (PaddleBall)newItem;
        }

        public static Samus CreateSamus(PhysicalScene scene)
        {
            newItem = new Samus(scene);
            newItem.Body.Position = new Vector2(
                ConvertUnits.ToSimUnits(WindowSize.X * 0.20f),
                ConvertUnits.ToSimUnits(WindowSize.Y - (newItem.Texture.Height)));

            return (Samus)newItem;
        }

        /// <summary>
        /// Creates and returns a paddle placed 95% above the bottom of the window, in the middle.
        /// </summary>
        public static Paddle CreatePaddleCenterFloor(PhysicalScene scene)
        {
            Vector2 spawnPosition = new Vector2(WindowSize.X / 2, (WindowSize.Y * 0.95f));
            return CreatePaddle(scene, spawnPosition);
        }

        public static Paddle CreatePaddle(PhysicalScene scene, Vector2 spawnPosition)
        {
            Texture2D texture = BounceGame.ContentManager.Load<Texture2D>("obstacle");
            newItem = new Paddle(scene, texture, ConvertUnits.ToSimUnits(spawnPosition));
            return (Paddle)newItem;
        }

        public static Obstacle CreateObstacle(PhysicalScene scene)
        {
            Texture2D obstacleTexture = BounceGame.ContentManager.Load<Texture2D>("obstacle");
            newItem = new Obstacle(scene, obstacleTexture);

            return (Obstacle)newItem;
        }

        public static Obstacle CreateObstacle(PhysicalScene scene, Vector2 spawnPosition)
        {
            Obstacle o = CreateObstacle(scene);
            o.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);
            o.Initialize();

            return o;
        }

        public static Metroid CreateMetroid(PhysicalScene scene)
        {
            Texture2D texture = BounceGame.ContentManager.Load<Texture2D>("metroid");
            newItem = new Metroid(scene, texture);

            return (Metroid)newItem;
        }

        public static Metroid CreateMetroid(PhysicalScene scene, Vector2 spawnPosition)
        {
            Metroid m = CreateMetroid(scene);
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return m;
        }

        public static Metroid CreateMetroid(PhysicalScene scene, Vector2 spawnPosition, float sinRadius, float cosRadius)
        {
            Texture2D texture = BounceGame.ContentManager.Load<Texture2D>("metroid");
            Metroid m = new Metroid(scene, texture, sinRadius, cosRadius);
            newItem = m;
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return m;
        }

        public static RectangleItem CreateRectangleItem(PhysicalScene scene, float width, float height)
        {
            newItem = new RectangleItem(scene, width, height);
            return (RectangleItem)newItem;
        }

        public static RectangleItem CreateRectangleItem(PhysicalScene scene, int width, int height)
        {
            return CreateRectangleItem(scene, (float)width, (float)height);
        }

        public static Brick CreateBrick(PhysicalScene scene)
        {
            Texture2D texture = BounceGame.ContentManager.Load<Texture2D>("brick");
            newItem = new Brick(scene, texture);
            return (Brick)newItem;
        }

        public static Brick CreateBrick(PhysicalScene scene, Vector2 spawnPosition)
        {
            Brick b = CreateBrick(scene);
            b.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return b;
        }
    }

    public static class ItemStructures
    {
        public static List<Metroid> MetroidsNearItems(PhysicalScene scene, ICollection<PhysicalItem> items, Vector2 offsetFromItem, int percentChance)
        {
            var metroidList = new List<Metroid>();

            if (items != null)
            {
                foreach (PhysicalItem item in items)
                    if (BounceGame.r.Next(1, 101) < percentChance)
                    {
                        metroidList.Add(ItemFactory.CreateMetroid(scene, new Vector2(
                            ConvertUnits.ToDisplayUnits(item.Body.Position.X) + offsetFromItem.X,
                            ConvertUnits.ToDisplayUnits(item.Body.Position.Y) - offsetFromItem.Y)));
                    }
            }

            return metroidList;
        }

        public static List<RectangleItem> CreateFraming(PhysicalScene scene, Vector2 frameSize, int width)
        {
            var rectangles = new List<RectangleItem>();

            float x = frameSize.X;
            float y = frameSize.Y;

            //The following algorithm is absolutely ridiculous, and was written purely for fun.
            //That said, it works perfectly.
            for (float i = 0f; i <= x; i += x)
            {
                for (float j = 0f; j <= y; j += y)
                {
                    var newItem = ItemFactory.CreateRectangleItem(scene,
                        ConvertUnits.ToSimUnits(i == 0 ? (int)x : width),
                        ConvertUnits.ToSimUnits(i == 0 ? width : (int)y));

                    Vector2 position = new Vector2(
                        i == 0 ? x / 2 : (i + (j == 0 ? width / 2 : -i + (-width / 2))),
                        j == 0 ? (i == 0 ? -width / 2 : y / 2) : (i == 0 ? j + width / 2 : j / 2));

                    newItem.Body.Position = ConvertUnits.ToSimUnits(position);
                    newItem.Body.BodyType = BodyType.Static;
                    rectangles.Add(newItem);
                }
            }

            //Metroids Kill() on contact with floor.
            rectangles[1].Body.OnCollision += (Fixture fixtureA, Fixture fixtureB, Contact contact) =>
            {
                if (fixtureB.Body.UserData is Metroid)
                    (fixtureB.Body.UserData as Metroid).Kill();

                return true;
            };

            return rectangles;
        }

        public static List<Obstacle> CreateRandomlyPositionedObstacles(PhysicalScene scene, Vector2 frameSize, int number)
        {
            var obstacleList = new List<Obstacle>();
            var rectangleList = new List<Rectangle>();
            Texture2D texture = BounceGame.ContentManager.Load<Texture2D>("obstacle");
            Vector2 spawnPositionOffset = new Vector2(texture.Width / 2, (texture.Height / 2) * 5f);

            for (int i = 0; i < number; i++)
            {
                Obstacle o = ItemFactory.CreateObstacle(scene,
                    VectorStructures.RandomPosition(frameSize, spawnPositionOffset));
                o.UpdatePosition();
                while (doesObstacleIntersect(o, rectangleList))
                {
                    o.Body.Position = ConvertUnits.ToSimUnits(
                        VectorStructures.RandomPosition(frameSize, spawnPositionOffset));
                    o.UpdatePosition();
                }

                obstacleList.Add(o);
                rectangleList.Add(o.Rectangle);
            }

            return obstacleList;
        }

        private static bool doesObstacleIntersect(Obstacle o, List<Rectangle> rectangleList)
        {
            foreach (Rectangle rectangleToTest in rectangleList)
            {
                rectangleToTest.Inflate(10, 25);
                if (o.Rectangle.Intersects(rectangleToTest))
                    return true;
            }

            return false;
        }

        public static List<Metroid> MetroidRow(PhysicalScene scene, int numberOfMetroids, Vector2 startingPosition, int pixelsApart)
        {
            var spawnPositions = VectorStructures.Row(numberOfMetroids, startingPosition, pixelsApart);
            var metroidList = new List<Metroid>();

            var unitCircle = new UnitCircle();
            int radiusIndex = 5;
            foreach (Vector2 spawnPosition in spawnPositions)
            {
                metroidList.Add(
                    ItemFactory.CreateMetroid(
                        scene, spawnPosition, (float)unitCircle.RadiansList.Values[radiusIndex], 0f));
                radiusIndex += 2;
            }

            return metroidList;
        }

        public static List<Metroid> MetroidColumn(PhysicalScene scene, int numberOfMetroids, Vector2 startingPosition, int pixelsApart)
        {
            var spawnPositions = VectorStructures.Column(numberOfMetroids, startingPosition, pixelsApart);
            var metroidList = new List<Metroid>((int)numberOfMetroids);

            var unitCircle = new UnitCircle();
            int radiusIndex = 2;
            foreach (Vector2 spawnPosition in spawnPositions)
            {
                metroidList.Add(
                    ItemFactory.CreateMetroid(
                        scene, spawnPosition, 0f, (float)unitCircle.RadiansList.Values[radiusIndex]));
                radiusIndex += 2;
            }

            return metroidList;
        }

        public static List<Brick> BrickRow(PhysicalScene scene, int numberOfBricks, Vector2 startingPosition, int pixelsApart)
        {
            var spawnPositions = VectorStructures.Row(numberOfBricks, startingPosition, pixelsApart);
            var brickList = new List<Brick>((int)numberOfBricks);

            foreach (Vector2 spawnPosition in spawnPositions)
                brickList.Add(ItemFactory.CreateBrick(scene, spawnPosition));

            return brickList;
        }
    }
}
