using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using Demiurg.Core;

namespace CoreMod
{
    public class ExtractSurfaceFromChunks  : Demiurg.Core.Avatar
    {
        [AInput ("main")]
        List<GameObject> mainI;
        [AOutput ("main")]
        List<TileRef[]> mainO;
        [AConfig ("target_surface")]
        int targetSurface;
        [AConfig ("filter_less")]
        int filterLess;

        public override void Work ()
        {
            mainO = new List<TileRef[]> ();
            for (int i = 0; i < mainI.Count; i++)
            {
                var input = mainI [i];
                ChunkSlot chunk = input.GetComponent<ChunkSlot> ();
                if (chunk.Surface == targetSurface)
                    mainO.Add (chunk.Tiles);
            }
            int k = 0;
            int count = mainO.Count;
            while (k < count)
            {
                if (mainO [k].Length < filterLess)
                {
                    mainO.RemoveAt (k);
                    count--;
                }
                else
                    k++;
            }

            FinishWork ();
        }
    
    }
}

