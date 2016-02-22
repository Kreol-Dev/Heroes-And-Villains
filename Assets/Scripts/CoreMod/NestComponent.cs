using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace CoreMod
{

	[ASlotComponent ("Nest")]
	[AShared]
	public class NestComponent : SlotComponent
	{

	}

	public class NestsSlotsModule : SlotsProcessor
	{
		public override void Work ()
		{
			OutputObjects = InputObjects;
			foreach (var inputObject in InputObjects)
				inputObject.AddComponent<NestComponent> ();
			FinishWork ();
		}

	}

}


