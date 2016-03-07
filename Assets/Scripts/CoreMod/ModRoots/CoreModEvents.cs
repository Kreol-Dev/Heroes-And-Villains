using UnityEngine;
using System.Collections;
using Signals;

namespace CoreMod
{
	public class CoreModEvents : ModRoot
	{

		public Signal<SpatialObject> SpatialObjectCreated;
		public Signal<SpatialObject> SpatialObjectDestroyed;

		protected override void CustomSetup ()
		{
			SpatialObjectCreated = new Signal<SpatialObject> ();
			SpatialObjectDestroyed = new Signal<SpatialObject> ();
			Fulfill.Dispatch ();
		}
	}
}


