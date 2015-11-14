using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;


namespace CoreMod
{
    public class LatitudeModule : CreationNode
    {
        NodeOutput<int[,]> mainO;
        IntParam polarValue;
        IntParam centralValue;
        IntParam width;
        IntParam height;
        protected override void SetupIOP ()
        {
            mainO = Output<int[,]> ("main");
            polarValue = Config<IntParam> ("polar_value");
            centralValue = Config<IntParam> ("central_value");
            width = Config<IntParam> ("width");
            height = Config<IntParam> ("height");
        }

        protected override void Work ()
        {
            float polar = (float)polarValue.Content;
            float central = (float)centralValue.Content;
            int[,] latitudeMap = new int[width, height];
            float halfHeight = height / 2;
            for (int j = 0; j < height; j++)
            {
                int value = (int)Mathf.Lerp (central, polar, Mathf.Abs (halfHeight - j) / halfHeight);
                for (int i = 0; i < width; i++)
                    latitudeMap [i, j] = value;

            }
            mainO.Finish (latitudeMap);
        }
        
        
    }
}
