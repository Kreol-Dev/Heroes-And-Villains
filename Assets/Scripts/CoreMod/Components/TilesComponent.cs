using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoreMod
{
	public class TilesComponent : EntityComponent
	{
		public override void LoadFromTable (Demiurg.Core.Extensions.ITable table)
		{
			
		}

		public override void CopyTo (GameObject go)
		{
			go.AddComponent<TilesComponent> ().tiles = tiles;
		}

		public List<TileHandle> tiles;
	}

}


