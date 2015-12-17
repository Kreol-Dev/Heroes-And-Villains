
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using UnityEngine;
using Demiurg.Core;


namespace CoreMod
{
    public class DistinctorModule  : Demiurg.Core.Avatar
    {
       

        class LevelPair
        {
            public float Level;
            public int Value;
        }

        [AOutput ("main")]
        int[,] mainO;
        [AInput ("main")]
        float[,] mainI;
        [AConfig ("levels")]
        List<LevelPair> levels;

        int FindLevel (float value)
        {
            for (int i = 0; i < levels.Count - 1; i++)
            {
                if (levels [i + 1].Level >= value && levels [i].Level <= value)
                {
                    //Debug.LogFormat ("{2} <= {1} <= {0} ----> {3}", pairs [i + 1].Level, value, pairs [i].Level, pairs [i + 1].Value);
                    return levels [i + 1].Value;
                }
                    
            }
            if (value > levels [levels.Count - 1].Level)
                return levels [levels.Count - 1].Value;
            return levels [0].Value;
        }


        public override void Work ()
        {
            var array = new int[mainI.GetLength (0), mainI.GetLength (1)];
            for (int i = 0; i < array.GetLength (0); i++)
                for (int j = 0; j < array.GetLength (1); j++)
                {
                    array [i, j] = FindLevel (mainI [i, j]);
                }
            FinishWork ();

        }
    }
}



