using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;

namespace CoreMod
{
    public class ExtractSurfaceFromChunks : CreationNode
    {
        NodeInput<List<GameObject>> mainI;
        NodeOutput<List<TileRef[]>> mainO;
        IntParam targetSurface;
        IntParam filterLess;
        protected override void SetupIOP ()
        {
            mainI = Input<List<GameObject>> ("main");
            mainO = Output<List<TileRef[]>> ("main");
            targetSurface = Config<IntParam> ("target_surface");
            filterLess = Config<IntParam> ("filter_less");
        }
        protected override void Work ()
        {
            mainO.Content = new List<TileRef[]> ();
            if (targetSurface.Undefined)
            {
                for (int i = 0; i < mainI.Content.Count; i++)
                {
                    var input = mainI.Content [i];
                    ChunkSlot chunk = input.GetComponent<ChunkSlot> ();
                    mainO.Content.Add (chunk.Tiles);
                }
            }
            else
            {
                for (int i = 0; i < mainI.Content.Count; i++)
                {
                    var input = mainI.Content [i];
                    ChunkSlot chunk = input.GetComponent<ChunkSlot> ();
                    if (chunk.Surface == targetSurface)
                        mainO.Content.Add (chunk.Tiles);
                }
            }
            if (!filterLess.Undefined)
            {
                int i = 0;
                int count = mainO.Content.Count;
                while (i < count)
                {
                    if (mainO.Content [i].Length < filterLess)
                    {
                        mainO.Content.RemoveAt (i);
                        count--;
                    }
                    else
                        i++;
                }
            }


            mainO.Finish ();
        }
    
    }
}

