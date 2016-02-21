using UnityEngine;
using System.Collections;

namespace AI
{
	[RootDependencies (typeof(ModsManager))]
	public class AIRoot : Root
	{
		protected override void CustomSetup ()
		{
			Fulfill.Dispatch ();
		}




	}
}


