using UnityEngine;
using System.Collections;
using Demiurg;

namespace CoreMod
{
    public class ArrayBlendModule : CreationNode
    {
        NodeInput<int[,]> secondI;
        NodeInput<int[,]> firstI;
        NodeOutput<int[,]> mainO;
        FloatParam weight;
        protected override void SetupIOP ()
        {
            firstI = Input<int[,]> ("first");
            secondI = Input<int[,]> ("second");
            mainO = Output<int[,]> ("main");
            weight = Config<FloatParam> ("weight");
        }
        
        protected override void Work ()
        {
            int width = firstI.Content.GetLength (0);
            int height = firstI.Content.GetLength (1);
            int[,] blendedMap = new int[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    blendedMap [i, j] = (int)Mathf.Lerp (firstI.Content [i, j], secondI.Content [i, j], 1f - weight);
                }
            mainO.Finish (blendedMap);
        }
        
        
    }
}

