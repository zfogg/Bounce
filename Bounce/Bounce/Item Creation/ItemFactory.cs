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
        public static Samus CreateSamus(PhysicalScene scene)
        {
            return new Samus(scene);
        }

        public static Samus CreateSamus(PhysicalScene scene, Vector2 spawnPosition)
        {
            var s = CreateSamus(scene);
            s.Body.Position = spawnPosition;

            return s;
        }

        public static Paddle CreatePaddle(PhysicalScene scene, Vector2 spawnPosition)
        {
            var texture = BounceGame.ContentManager.Load<Texture2D>("obstacle");
            return new Paddle(scene, texture, ConvertUnits.ToSimUnits(spawnPosition));
        }

        public static PaddleBall CreatePaddleBall(PhysicalScene scene, Paddle paddle)
        {
            var texture = BounceGame.ContentManager.Load<Texture2D>("dragonBall");
            return new PaddleBall(scene, paddle, texture);
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
            return new Brick(scene, texture);
        }

        public static Brick CreateBrick(PhysicalScene scene, Vector2 spawnPosition)
        {
            var b = CreateBrick(scene);
            b.Body.Position = ConvertUnits.ToSimUnits(spawnPosition);

            return b;
        }
    }
}
