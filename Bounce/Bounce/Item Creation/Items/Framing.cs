using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


namespace Bounce
{
    public class Framing
    {
        public Framing(World world, Viewport viewPort)
        {
            BackgroundTexture = BounceGame.ContentManager.Load<Texture2D>("background");

            //FloorBody.Restitution = 1f;
            //CielingBody.Restitution = 1f;
            //RightSideBody.Restitution = 1f;
            //LeftSideBody.Restitution = 1f;

            Create(world, new Vector2(viewPort.Width, viewPort.Height), 5);
        }

        private void Create(World world, Vector2 screenSize, float width)
        {
            List<Body> bodyList = new List<Body>();

            //Floor and cieling
            for (float i = 0f; i <= screenSize.Y; i += screenSize.Y)
            {
                Body newBody = BodyFactory.CreateRectangle(world,
                    ConvertUnits.ToSimUnits(screenSize.X),
                    ConvertUnits.ToSimUnits(width), 1f);
                
                newBody.Position = new Vector2(
                    ConvertUnits.ToSimUnits(screenSize.X / 2),
                    ConvertUnits.ToSimUnits(i + ((i == 0 ? -1 : 1) * (width / 2))));

                newBody.BodyType = BodyType.Static;
                bodyList.Add(newBody);
            }

            //Right and left walls
            for (float i = 0f; i <= screenSize.X; i += screenSize.X)
            {
                Body newBody = BodyFactory.CreateRectangle(world,
                    ConvertUnits.ToSimUnits(width),
                    ConvertUnits.ToSimUnits(screenSize.Y), 1f);
                
                newBody.Position = new Vector2(
                    ConvertUnits.ToSimUnits(i + ((i == 0 ? -1 : 1) * (width / 2))),
                    ConvertUnits.ToSimUnits(screenSize.Y / 2));

                newBody.BodyType = BodyType.Static;
                bodyList.Add(newBody);
            }
        }

        public static Texture2D BackgroundTexture;

        public void Draw(SpriteBatch spriteBatch)
        {
             spriteBatch.Draw(BackgroundTexture, Vector2.Zero, Color.White);
        }
    }
}
