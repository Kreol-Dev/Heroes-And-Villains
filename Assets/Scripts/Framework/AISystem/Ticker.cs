using UnityEngine;
using System.Collections;

namespace AI
{
	public class Ticker : Root
	{
		public delegate void VoidDelegate ();

		public event VoidDelegate Tick;

		public float TickDelta = 2f;

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
				yield return new WaitForSeconds (TickDelta);
			}
		}

	}
}

