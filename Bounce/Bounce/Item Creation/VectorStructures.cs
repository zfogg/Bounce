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
    public static class VectorStructures
    {
        public static List<Vector2> HorizontalRow(int numberOfPositions, Vector2 startingPosition, int pixelsApart)
        {
            
            List<Vector2> positions = new List<Vector2>();
            positions.Add(startingPosition);

            Vector2 position = startingPosition;
            for (int i = 0; i < numberOfPositions * pixelsApart; i += pixelsApart)
            {
                position.X += (float)pixelsApart;
                positions.Add(position);
            }

            return positions;
        }

        public static List<Vector2> VerticalRow(int numberOfPositions, Vector2 startingPosition, int pixelsApart)
        {
            List<Vector2> positions = new List<Vector2>();
            positions.Add(startingPosition);

            Vector2 position = startingPosition;
            for (int i = 0; i < numberOfPositions * pixelsApart; i += pixelsApart)
            {
                position.Y += (float)pixelsApart;
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

        public static List<Vertices> TextureToBayazitList(Texture2D texture)
        {
            Vertices vertices = TextureToVertices(texture);

            Vector2 centroid = -vertices.GetCentroid();
            vertices.Translate(ref centroid);
            vertices = SimplifyTools.ReduceByDistance(vertices, 10f);

            List<Vertices> verticesList = BayazitDecomposer.ConvexPartition(vertices);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));

            foreach (Vertices v in verticesList)
                v.Scale(ref vertScale);

            return verticesList;
        }
    }
}
