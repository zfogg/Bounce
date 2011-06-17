using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


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
