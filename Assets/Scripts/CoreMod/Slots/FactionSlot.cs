using UnityEngine;
using System.Collections;
using Demiurg;
using UIO;
using System.Collections.Generic;

namespace CoreMod
{
	[ASlotComponent ("Faction")]
	[AShared]
	public class FactionSlot : SlotComponent
	{
		public List<GameObject> Ownership;
	}

}
