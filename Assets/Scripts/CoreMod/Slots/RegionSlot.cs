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
		public List<TileHandle> Tiles;

		public override void FillComponent (GameObject go)
		{
			var slots = go.GetComponents<ISlotted<RegionSlot>> ();
			foreach (var slot in slots)
			{
				slot.Receive (this);
			}

		}
	}

}

