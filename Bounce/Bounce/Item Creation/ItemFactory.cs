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
        public static PaddleBall CreatePaddleBall(PhysicalScene scene, Paddle paddle)
        {
            var texture = BounceGame.ContentManager.Load<Texture2D>("dragonBall");
            var pb = new PaddleBall(scene, paddle, texture);
            pb.Body.Position = paddle.Body.Position + ConvertUnits.ToSimUnits((-Vector2.UnitY * 50f));
            return pb;
        }

        public static Samus CreateSamus(PhysicalScene scene, Vector2 spawnPosition)
        {
            var s = new Samus(scene);
            s.Body.Position = spawnPosition;

            return s;
        }

        public static Paddle CreatePaddle(PhysicalScene scene, Vector2 spawnPosition)
        {
            var texture = BounceGame.ContentManager.Load<Texture2D>("obstacle");
            var p = new Paddle(scene, texture, ConvertUnits.ToSimUnits(spawnPosition));
            return p;
        }

        public static Obstacle CreateObstacle(PhysicalScene scene)
        {
            var obstacleTexture = BounceGame.ContentManager.Load<Texture2D>("obstacle");
            var o = new Obstacle(scene, obstacleTexture);

            return o;
        }

        public static Obstacle CreateObstacle(PhysicalScene scene, Vector2 spawnPosition)
        {
            var o = CreateObstacle(scene);
            o.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);
            o.Initialize();

            return o;
        }

        public static Metroid CreateMetroid(PhysicalScene scene)
        {
            var texture = BounceGame.ContentManager.Load<Texture2D>("metroid");
            var m = new Metroid(scene, texture);

            return m;
        }

        public static Metroid CreateMetroid(PhysicalScene scene, Vector2 spawnPosition)
        {
            var m = CreateMetroid(scene);
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return m;
        }

        public static Metroid CreateMetroid(PhysicalScene scene, Vector2 spawnPosition, float sinRadius, float cosRadius)
        {
            var texture = BounceGame.ContentManager.Load<Texture2D>("metroid");
            var m = new Metroid(scene, texture, sinRadius, cosRadius);
            m.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return m;
        }

        public static RectangleItem CreateRectangleItem(PhysicalScene scene, float width, float height)
        {
            var r = new RectangleItem(scene, width, height);
            return r;
        }

        public static RectangleItem CreateRectangleItem(PhysicalScene scene, int width, int height)
        {
            return CreateRectangleItem(scene, (float)width, (float)height);
        }

        public static Brick CreateBrick(PhysicalScene scene)
        {
            var texture = BounceGame.ContentManager.Load<Texture2D>("brick");
            var b = new Brick(scene, texture);
            return b;
        }

        public static Brick CreateBrick(PhysicalScene scene, Vector2 spawnPosition)
        {
            var b = CreateBrick(scene);
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
