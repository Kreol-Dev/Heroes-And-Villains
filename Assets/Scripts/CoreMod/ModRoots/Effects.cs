using UnityEngine;
using System.Collections;
using System;

namespace CoreMod
{
	public class Effects : ModRoot
	{

		protected override void CustomSetup ()
		{
			Fulfill.Dispatch ();
		}

		protected override void PreSetup ()
		{
			base.PreSetup ();
		}

		public Effect GetEffect<T> () where T : Effect
		{
			return null;
		}

		public Effect GetEffect (Type type)
		{
			return null;
		}
	}


	public class Effect
	{
		
		public void Apply (GameObject go)
		{
			
		}

		public void Reverse (GameObject go)
		{
			
		}
	}

}


