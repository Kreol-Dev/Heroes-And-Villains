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
		[Defined ("height")]
		public float Height;
		[Defined ("temperature")]
		public float Temperature;
		[Defined ("humidity")]
		public float Humidity;
		[Defined ("radioactivity")]
		public float Radioactivity;
		[Defined ("inlandness")]
		public float Inlandness;
	}
}


