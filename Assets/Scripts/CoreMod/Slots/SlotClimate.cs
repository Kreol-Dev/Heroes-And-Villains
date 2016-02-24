using UnityEngine;
using System.Collections;
using Demiurg;
using MoonSharp.Interpreter;
using UIO;


namespace CoreMod
{
	[ASlotComponent ("Climate")]
	[AShared]
	public class SlotClimate : SlotComponent
	{
		public float Height;
		public float Temperature;
		public float Humidity;
		public float Radioactivity;
		public float Inlandness;
	}
}


