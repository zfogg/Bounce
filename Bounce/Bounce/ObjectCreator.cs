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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ObjectCreator : Microsoft.Xna.Framework.GameComponent
    {
        private Game game;
        public ObjectCreator(Game game)
            : base(game)
        {
            r = new Random();
            this.game = game;
            //game.Components.Add(this);
        }
        Random r;

        private List<Body> bodies;
        public override void Initialize()
        {
            r = new Random();
            base.Initialize();
        }

        public List<Obstacle> CreateObstacles(int number)
        {
            List<Obstacle> obstacleList = new List<Obstacle>(100);
            for (int i = 0; i < number && i < BounceGame.CreationLimit; i++)
            {
                Vector2 position = new Vector2( //This controls where the obstacles can spawn.
                    ConvertUnits.ToSimUnits(r.Next( //X axis.
                        (0 + Obstacle.Texture.Width), //Left: spawn fully inside the screen by at least the obstacle's texture width.
                        (BounceGame.Graphics.PreferredBackBufferWidth - Obstacle.Texture.Width))), //Right: spawn fully inside the screen by at least the obstacle's texture width.
                    ConvertUnits.ToSimUnits(r.Next( //Y axis.
                        (int)((float)BounceGame.Graphics.PreferredBackBufferHeight * 0.20f), //Top: spawn below x% of the screen's height.
                        (BounceGame.Graphics.PreferredBackBufferHeight - (Obstacle.Texture.Height * 2)))) //Bottom: spawn above the floor by two of obstacle's texture height.
                );
                Obstacle o = new Obstacle(game, position);
                obstacleList.Add(o);
            }

            return obstacleList;
        }

        public List<Metroid> CreateMetroidsOnObstacles(ref List<Obstacle> obstacles, int percentchance)
        {
            List<Metroid> metroidList = new List<Metroid>(100);
            foreach (Obstacle obstacle in obstacles)
                if (r.Next(1, 101) > percentchance)
                {
                    Metroid m = new Metroid(game);

                    m.Body.Position = new Vector2(
                        obstacle.Body.Position.X,
                        ConvertUnits.ToSimUnits(ConvertUnits.ToDisplayUnits(obstacle.Body.Position.Y) - (1.2f * (float)Metroid.Texture.Height)
                        ));

                    metroidList.Add(m);
                }

            return metroidList;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
