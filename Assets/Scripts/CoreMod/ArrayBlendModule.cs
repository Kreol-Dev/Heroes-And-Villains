using UnityEngine;
using System.Collections;
using Demiurg.Core;

namespace CoreMod
{
    public class ArrayBlendModule : Demiurg.Core.Avatar
    {
        [AInput ("second")]
        int[,] secondI;
        [AInput ("first")]
        int[,] firstI;
        [AOutput ("main")]
        int[,] mainO;
        [AConfig ("weight")]
        float weight;

        public override void Work ()
        {
            int width = firstI.GetLength (0);
            int height = firstI.GetLength (1);
            int[,] blendedMap = new int[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    blendedMap [i, j] = (int)Mathf.Lerp (firstI [i, j], secondI [i, j], 1f - weight);
                }
            mainO = blendedMap;
            base.FinishWork ();
        }

        
    }
}

