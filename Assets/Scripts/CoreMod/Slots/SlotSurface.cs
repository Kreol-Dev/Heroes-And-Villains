using UnityEngine;
using System.Collections;
using UIO;


namespace CoreMod
{
	[AShared]
	[ASlotComponent ("Surface")]
	public class SlotSurface : SlotComponent
	{
		[Defined ("surface_id")]
		public int SurfaceID;
	}

}

