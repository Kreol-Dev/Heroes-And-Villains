using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public class Modifiers : ModRoot
	{

		protected override void CustomSetup ()
		{
			Fulfill.Dispatch ();
		}

		protected override void PreSetup ()
		{
			base.PreSetup ();
		}
			
	}


	public class Modifier<T> : Effect where T : EntityComponent
	{
		HashSet<T> targets = new HashSet<T> ();

		//		public void AttachTo (T target)
		//		{
		//			if (targets.Add (target))
		//			{
		//				target.Destroyed += DetachFrom;
		//			}
		//		}
		//
		//		public void DetachFrom (T target)
		//		{
		//			if (targets.Remove (target))
		//			{
		//				target.Destroyed -= DetachFrom;
		//			}
		//		}
		//
		//

	}
}


