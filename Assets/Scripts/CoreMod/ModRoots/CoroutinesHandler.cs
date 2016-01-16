using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class CoroutinesHandler : ModRoot
	{
		#region implemented abstract members of Root

		protected override void CustomSetup ()
		{
			Fulfill.Dispatch ();
		}

		#endregion


	}

}

