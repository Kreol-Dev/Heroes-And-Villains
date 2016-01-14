using UnityEngine;
using System.Collections;
using Demiurg;
using Demiurg.Core;

namespace CoreMod
{
    public class FloatToIntArray : Demiurg.Core.Avatar
    {
        [AOutput ("main")]
        int[,] mainO;
        [AInput ("main")]
        float[,] mainI;
        [AConfig ("max_value")]
        int maxValue;
        [AConfig ("min_value")]
        int minValue;

        
        
        public override void Work ()
        {
            var array = new int[mainI.GetLength (0), mainI.GetLength (1)];
            float maxInputValue = float.MinValue;
            float minInputValue = float.MaxValue;
            for (int i = 0; i < array.GetLength (0); i++)
                for (int j = 0; j < array.GetLength (1); j++)
                {
                    if (mainI [i, j] > maxInputValue)
                        maxInputValue = mainI [i, j];
                    if (mainI [i, j] < minInputValue)
                        minInputValue = mainI [i, j];
                }

            for (int i = 0; i < array.GetLength (0); i++)
                for (int j = 0; j < array.GetLength (1); j++)
                {
                    array [i, j] = (int)(Mathf.Lerp (minValue, maxValue, Mathf.InverseLerp (minInputValue, maxInputValue, mainI [i, j])));
                }
            mainO = array;
            FinishWork ();
            
        }
    }
}

