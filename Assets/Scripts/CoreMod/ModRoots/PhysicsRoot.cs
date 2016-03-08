using UnityEngine;
using System.Collections;

namespace CoreMod
{
	[RootDependencies (typeof(MapRoot.Map))]
	public class PhysicsRoot : ModRoot
	{
		public const int MaterialObjectsLayer = 10;
		public const int TriggerObjectsLayer = 11;
		public const int RegionObjectsLayer = 12;
		public int MapColliderLayer;

		protected override void CustomSetup ()
		{
			Fulfill.Dispatch ();
		}

		protected override void PreSetup ()
		{
			MapColliderLayer = LayerMask.NameToLayer ("MapCollider");
			Physics2D.IgnoreLayerCollision (TriggerObjectsLayer, TriggerObjectsLayer, true);
			Physics2D.IgnoreLayerCollision (RegionObjectsLayer, RegionObjectsLayer, true);
			Physics2D.IgnoreLayerCollision (TriggerObjectsLayer, RegionObjectsLayer, true);

			Physics2D.IgnoreLayerCollision (MaterialObjectsLayer, MapColliderLayer, true);
			Physics2D.IgnoreLayerCollision (RegionObjectsLayer, MapColliderLayer, true);
			Physics2D.IgnoreLayerCollision (TriggerObjectsLayer, MapColliderLayer, true);
		}
	}
}


