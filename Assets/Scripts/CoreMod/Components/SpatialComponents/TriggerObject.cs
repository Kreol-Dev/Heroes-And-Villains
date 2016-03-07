using UnityEngine;
using System.Collections;
using UIO;

namespace CoreMod
{
	[AShared]
	[ECompName ("spatial_trigger")]
	public class TriggerObject : MaterialObject
	{
		protected override void Init ()
		{
			base.Init ();
			gameObject.layer = PhysicsRoot.TriggerObjectsLayer;
		}

	}

}