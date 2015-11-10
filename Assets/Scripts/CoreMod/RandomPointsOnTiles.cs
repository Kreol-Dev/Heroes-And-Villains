using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;

namespace CoreMod
{
    public class RandomPointsOnTiles : CreationNode
    {
        NodeInput<List<TileRef[]>> mainI;
        NodeOutput<TileRef[]> mainO;
        IntParam density;
        protected override void SetupIOP ()
        {
            mainI = Input<List<TileRef[]>> ("main");
            mainO = Output<TileRef[]> ("main");
            density = Config<IntParam> ("density");
        }
        protected override void Work ()
        {

            int pointsCount = 0;
            for (int i = 0; i < mainI.Content.Count; i++)
                pointsCount += mainI.Content [i].Length / density + 1;
            TileRef[] points = new TileRef[pointsCount];
            int curPoint = 0;
            for (int i = 0; i < mainI.Content.Count; i++)
            {
                TileRef[] tiles = mainI.Content [i];
                int localCount = tiles.Length / density + 1;
                int chunkRange = tiles.Length / localCount;
                for (int j = 0; j <= tiles.Length - chunkRange; j += chunkRange)
                {
                    points [curPoint++] = tiles [Random.Next (j, j + chunkRange)];
                }
            }
            mainO.Finish (points);
        }
        
    }

}

