
using System.Collections;
using System.Collections.Generic;
using Demiurg;


namespace CoreMod
{
    public class DistinctorModule : CreationNode
    {
        class LevelValue
        {
            public FloatParam Level = new FloatParam ("level");
            public IntParam Value = new IntParam ("val");
        }
        struct LevelPair
        {
            public float Level;
            public int Value;
        }
        NodeOutput<int[,]> mainO; 
        NodeInput<float[,]> mainI;
        protected override void SetupIOP ()
        {
            mainO = Output<int[,]> ("main");
            mainI = Input<float[,]> ("main");
            levels = Config<GlobalArrayParam<LevelValue>> ("levels");
        }
        GlobalArrayParam<LevelValue> levels;
        LevelPair[] pairs;
        int FindLevel (float value)
        {
            for (int i = 0; i < pairs.Length - 1; i++)
            {
                if (pairs [i + 1].Level >= value && pairs [i].Level <= value)
                    return pairs [i].Value;
            }
            return pairs [0].Value;
        }


        protected override void Work ()
        {
            pairs = new LevelPair[levels.Content.Length];
            for (int i = 0; i < levels.Content.Length; i++)
            {
                pairs [i].Level = levels.Content [i].Level;
                pairs [i].Value = levels.Content [i].Value;
            }
            var array = new int[mainI.Content.GetLength (0), mainI.Content.GetLength (1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array [i, j] = FindLevel (mainI.Content [i, j]);
                }
            mainO.Finish (array);

        }
    }
}



