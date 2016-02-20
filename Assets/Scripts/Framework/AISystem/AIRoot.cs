using UnityEngine;
using System.Collections;

[RootDependencies (typeof(ModsManager))]
public class AIRoot : Root
{
	protected override void CustomSetup ()
	{

		var mm = Find.Root<ModsManager> ();
		var allTypes = mm.GetAllTypes ();


		Fulfill.Dispatch ();
	}

}

