using UnityEngine;
using System.Collections;
using Demiurg;

namespace CoreMod
{
    public class FloatToIntArray : CreationNode
    {
        NodeOutput<int[,]> mainO; 
        NodeInput<float[,]> mainI;
        IntParam maxValue;
        IntParam minValue;
        protected override void SetupIOP ()
        {
            mainO = Output<int[,]> ("main");
            mainI = Input<float[,]> ("main");
            maxValue = Config<IntParam> ("max_value");
            minValue = Config<IntParam> ("min_value");
        }

        
        
        protected override void Work ()
        {
            var array = new int[mainI.Content.GetLength (0), mainI.Content.GetLength (1)];
            float maxInputValue = float.MinValue;
            float minInputValue = float.MaxValue;
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (mainI.Content [i, j] > maxInputValue)
                        maxInputValue = mainI.Content [i, j];
                    if (mainI.Content [i, j] < minInputValue)
                        minInputValue = mainI.Content [i, j];
                }

            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array [i, j] = (int)(Mathf.Lerp (minValue, maxValue, Mathf.InverseLerp (minInputValue, maxInputValue, mainI.Content [i, j])));
                }
            mainO.Finish (array);
            
        }
    }
}

