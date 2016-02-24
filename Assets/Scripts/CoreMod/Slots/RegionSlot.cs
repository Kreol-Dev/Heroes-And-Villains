using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;

namespace CoreMod
{
	[ASlotComponent ("Region")]
	[AShared]
	public class RegionSlot : SlotComponent
	{
		[Defined ("size")]
		public int Size { get { return Tiles.Count; } }

		[Defined ("is_region")]
		public bool IsRegion = false;
		public List<TileHandle> Tiles;
		public string TargetLayerName;

		public override void FillComponent (GameObject go)
		{
			var tiles = go.AddComponent<TilesComponent> ();
			tiles.Receive (this);

		}
	}

}

