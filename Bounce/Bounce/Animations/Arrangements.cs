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
    public static class Arrangements
    {
        public static List<Vector2> HorizontalRow(int numberofpositions, Vector2 startingposition, int pixelsapart)
        {
            
            List<Vector2> positions = new List<Vector2>();
            positions.Add(startingposition);

            Vector2 position = startingposition;
            for (int i = 0; i < numberofpositions * pixelsapart; i += pixelsapart)
            {
                position.X += (float)pixelsapart;
                positions.Add(position);
            }

            return positions;
        }
    }
}
