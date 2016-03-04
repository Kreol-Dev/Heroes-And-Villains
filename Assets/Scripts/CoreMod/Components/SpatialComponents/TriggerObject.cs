using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class TriggerObject : MaterialObject
	{
		protected override void Init ()
		{
			base.Init ();
			gameObject.layer = PhysicsRoot.TriggerObjectsLayer;
		}

	}

}