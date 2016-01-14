using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using Demiurg.Core;

namespace CoreMod
{
    public class RandomPointsOnTiles : Demiurg.Core.Avatar
    {
        [AInput ("main")]
        List<TileRef[]> mainI;
        [AOutput ("main")]
        TileRef[] mainO;
        [AConfig ("density")]
        int density;

        public override void Work ()
        {

            int pointsCount = 0;
            for (int i = 0; i < mainI.Count; i++)
                pointsCount += mainI [i].Length / density + 1;
            TileRef[] points = new TileRef[pointsCount];
            int curPoint = 0;
            for (int i = 0; i < mainI.Count; i++)
            {
                TileRef[] tiles = mainI [i];
                int localCount = tiles.Length / density + 1;
                int chunkRange = tiles.Length / localCount;
                for (int j = 0; j <= tiles.Length - chunkRange; j += chunkRange)
                {
                    points [curPoint++] = tiles [Random.Next (j, j + chunkRange)];
                }
            }
            mainO = points;
            FinishWork ();
        }
        
    }

}

