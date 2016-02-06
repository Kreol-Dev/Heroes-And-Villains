using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	[ECompName ("province")]
	public class Province : EntityComponent
	{
		public override void PostCreate ()
		{
			
		}

		public override void CopyTo (GameObject go)
		{
			Province prov = go.AddComponent<Province> ();
		}


		public override void LoadFromTable (ITable table)
		{
		}

	}

}

