using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using System;
using Demiurg.Core;


namespace CoreMod
{
    public class SlotsPlacer : Demiurg.Core.Avatar
    {
        [AInput ("points")]
        TileRef[] points;
        [AOutput ("slots")]
        List<GameObject> slots;

        public override void Work ()
        {
            slots = new List<GameObject> ();
            for (int i = 0; i < points.Length; i++)
            {
                GameObject go = new GameObject ("slot GO");
                SlotTile comp = go.AddComponent<SlotTile> ();
                comp.X = points [i].X;
                comp.Y = points [i].Y;
                slots.Add (go);
            }
            FinishWork ();
        }
    }
}


