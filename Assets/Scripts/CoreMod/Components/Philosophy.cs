using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class Philosophy : EntityComponent
	{
		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<Philosophy> ();
		}

		public override void PostCreate ()
		{
			
		}

		protected override void PostDestroy ()
		{
			
		}



	}
}



