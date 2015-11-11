using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using System;


namespace CoreMod
{
    public class SlotsPlacer : CreationNode
    {
        NodeInput<TileRef[]> points;
        NodeOutput<List<GameObject>> slots;
        StringParam slotTypeName;
        protected override void SetupIOP ()
        {
            points = Input<TileRef[]> ("points");
            slots = Output<List<GameObject>> ("slots");
            slotTypeName = Config<StringParam> ("slot_type");
        }

        protected override void Work ()
        {
            slots.Content = new List<GameObject> ();
            if (slotTypeName.Undefined)
            {
                slots.Finish (new List<GameObject> ());
                return;
            }
            Type slotType = Type.GetType (slotTypeName); 
            for (int i = 0; i < points.Content.Length; i++)
            {
                GameObject go = new GameObject (slotTypeName + " based GO");
                go.AddComponent (slotType);
                slots.Content.Add (go);
            }
            slots.Finish ();
        }
    }
}


