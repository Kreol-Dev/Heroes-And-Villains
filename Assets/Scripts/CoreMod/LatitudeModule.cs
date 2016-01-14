using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using Demiurg.Core;


namespace CoreMod
{
    public class LatitudeModule : Demiurg.Core.Avatar
    {
        [AOutput ("main")]
        int[,] mainO;
        [AConfig ("polar_value")]
        int polarValue;
        [AConfig ("central_value")]
        int centralValue;
        [AConfig ("width")]
        int width;
        [AConfig ("height")]
        int height;

        public override void Work ()
        {
            float polar = (float)polarValue;
            float central = (float)centralValue;
            int[,] latitudeMap = new int[width, height];
            float halfHeight = height / 2;
            for (int j = 0; j < height; j++)
            {
                int value = (int)Mathf.Lerp (central, polar, Mathf.Abs (halfHeight - j) / halfHeight);
                for (int i = 0; i < width; i++)
                    latitudeMap [i, j] = value;

            }
            mainO = latitudeMap;
            FinishWork ();
        }
        
        
    }
}
