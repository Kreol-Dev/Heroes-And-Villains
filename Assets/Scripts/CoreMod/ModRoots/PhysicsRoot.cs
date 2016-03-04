using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class PhysicsRoot : ModRoot
	{
		public const int MaterialObjectsLayer = 10;
		public const int TriggerObjectsLayer = 11;
		public const int RegionObjectsLayer = 12;

		protected override void CustomSetup ()
		{
			Fulfill.Dispatch ();
		}

		protected override void PreSetup ()
		{
			
			Physics2D.IgnoreLayerCollision (TriggerObjectsLayer, TriggerObjectsLayer, true);
			Physics2D.IgnoreLayerCollision (RegionObjectsLayer, RegionObjectsLayer, true);
		}
	}
}


