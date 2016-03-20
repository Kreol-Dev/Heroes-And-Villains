using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class Ticker : ModRoot
	{
		public delegate void VoidDelegate ();

		public event VoidDelegate Tick;

		protected override void PreSetup ()
		{
			base.PreSetup ();
		}

		protected override void CustomSetup ()
		{
			StartCoroutine (TickCoroutine ());
			Fulfill.Dispatch ();
		}

		IEnumerator TickCoroutine ()
		{
			while (true)
			{
				if (Tick != null)
					Tick ();
				yield return new WaitForSeconds (1f);
			}
		}

	}
}


