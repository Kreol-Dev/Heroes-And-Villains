using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	[ASlotComponent ("Region")]
	[AShared]
	public class RegionSlot : SlotComponent
	{
		public int Size;
		public List<TileHandle> Tiles;
		public string TargetLayerName;

		public override void FillComponent (GameObject go)
		{
			var tiles = go.AddComponent<TilesComponent> ();
			tiles.Receive (this);

		}
	}

}

