using UnityEngine;
using System.Collections;
using UIO;
using System.Collections.Generic;

namespace CoreMod
{
	[ECompName ("province")]
	public class Province : EntityComponent
	{
		public override EntityComponent CopyTo (GameObject go)
		{
			Province prov = go.AddComponent<Province> ();
			prov.tiles = this.tiles;
			return prov;
		}

		List<TileHandle> tiles;



		public override void LoadFromTable (ITable table)
		{
		}

		public override void PostCreate ()
		{

		}

		protected override void PostDestroy ()
		{

		}
	}

}

