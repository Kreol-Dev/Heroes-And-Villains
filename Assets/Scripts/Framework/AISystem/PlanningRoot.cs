using UnityEngine;
using System.Collections;

namespace AI
{
	[RootDependencies (typeof(ModsManager))]
	public class PlanningRoot : Root
	{
		protected override void CustomSetup ()
		{
			Fulfill.Dispatch ();
		}




	}
}


