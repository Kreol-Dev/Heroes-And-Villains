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
		TileHandle[] points;
		[AConfig ("target_layer")]
		string targetLayerName;
		[AOutput ("slots")]
		List<GameObject> slots;

		public override void Work ()
		{
			slots = new List<GameObject> ();
			for (int i = 0; i < points.Length; i++)
			{
				GameObject go = new GameObject ("slot GO");
				go.AddComponent<Slot> ();
				RegionSlot region = go.AddComponent<RegionSlot> ();
				region.TargetLayerName = targetLayerName;
				region.Tiles = new List<TileHandle> ();
				region.Tiles.Add (Find.Root<TilesRoot> ().MapHandle.GetHandle (points [i].X, points [i].Y));
				SlotTile comp = go.AddComponent<SlotTile> ();
				comp.X = points [i].X;
				comp.Y = points [i].Y;
				slots.Add (go);
			}
			FinishWork ();
		}
	}
}


