using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	[ASlotComponent ("Region")]
	[ATabled]
	public class RegionSlot : SlotComponent
	{
		public TileHandle[] Tiles;
	}

}

