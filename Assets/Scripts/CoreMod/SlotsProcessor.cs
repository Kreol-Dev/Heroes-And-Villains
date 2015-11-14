
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
namespace CoreMod
{
    public abstract class SlotsProcessor : CreationNode
    {
        protected NodeInput<List<GameObject>> InputObjects;
        protected NodeOutput<List<GameObject>> OutputObjects;
        protected override void SetupIOP ()
        {
            InputObjects = Input<List<GameObject>> ("main");
            OutputObjects = Output<List<GameObject>> ("main");
        }



    }
}
