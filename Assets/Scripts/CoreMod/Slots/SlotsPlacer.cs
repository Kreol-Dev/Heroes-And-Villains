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
        protected override void SetupIOP ()
        {
            points = Input<TileRef[]> ("points");
            slots = Output<List<GameObject>> ("slots");
        }

        protected override void Work ()
        {
            slots.Content = new List<GameObject> ();
            for (int i = 0; i < points.Content.Length; i++)
            {
                GameObject go = new GameObject ("slot GO");
                SlotTile comp = go.AddComponent<SlotTile> ();
                comp.X = points.Content [i].X;
                comp.Y = points.Content [i].Y;
                slots.Content.Add (go);
            }
            slots.Finish ();
        }
    }
}


