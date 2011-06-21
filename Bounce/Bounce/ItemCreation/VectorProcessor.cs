using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;


namespace Bounce
{
    public static class VectorProcessor
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

        public static Vertices TextureToVertices(Texture2D texture)
        {
            uint[] textureData = new uint[texture.Width * texture.Height];
            texture.GetData<uint>(textureData);

            Vertices textureVertices = PolygonTools.CreatePolygon(textureData, texture.Width);

            return textureVertices;
        }

        public static List<Vertices> VerticesToBayazitList(Texture2D texture)
        {
            Vertices vertices = TextureToVertices(texture);

            Vector2 centroid = -vertices.GetCentroid();
            vertices.Translate(ref centroid);
            vertices = SimplifyTools.ReduceByDistance(vertices, 4f);

            List<Vertices> verticesList = BayazitDecomposer.ConvexPartition(vertices);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));

            foreach (Vertices v in verticesList)
                v.Scale(ref vertScale);

            return verticesList;
        }
    }
}
