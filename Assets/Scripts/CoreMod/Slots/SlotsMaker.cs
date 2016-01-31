using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using System;
using Demiurg.Core;


namespace CoreMod
{
	public class SlotBuilder  : Demiurg.Core.Avatar
	{
		[AInput ("count")]
		int count;
		[AOutput ("slots")]
		List<GameObject> slots;

		public override void Work()
		{			
			slots = new List<GameObject> ();
			for (int i = 0; i < count; i++)
			{
				GameObject go = new GameObject ("slots GO");
				go.AddComponent<Slot> ();
				slots.Add (go);
			}
			FinishWork ();
		}
	}
}
