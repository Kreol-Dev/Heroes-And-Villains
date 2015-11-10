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
        protected override void SetupIOP ()
        {
            mainI = Input<List<GameObject>> ("main");
            mainO = Output<List<TileRef[]>> ("main");
            targetSurface = Config<IntParam> ("target_surface");
        }
        protected override void Work ()
        {
            mainO.Content = new List<TileRef[]> ();
            if (targetSurface.Undefined)
            {
                Debug.Log ("Undefined");
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


            mainO.Finish ();
        }
    
    }
}

