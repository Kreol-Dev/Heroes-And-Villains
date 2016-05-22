using UnityEngine;
using System.Collections;

namespace CoreMod
{
	public class Relations : EntityComponent
	{
		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<Relations> ();
		}

		public override void PostCreate ()
		{
			
		}

		protected override void PostDestroy ()
		{
			
		}




	}

}

