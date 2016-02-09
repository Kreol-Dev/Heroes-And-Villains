using UnityEngine;
using System.Collections;
using Demiurg;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;


namespace CoreMod
{
	[ASlotComponent ("Tile")]
	[AShared]
	public class SlotTile : SlotComponent
	{
		public int X;
		public int Y;

		public override void FillComponent (GameObject go)
		{
			this.transform.position = new Vector3 (X, Y, 0);
			go.GetComponent<Transform> ().position = new Vector3 (X, Y, 0);
		}
	}


	public interface ISlotted<T>
	{
		void Receive (T data);
	}
}


