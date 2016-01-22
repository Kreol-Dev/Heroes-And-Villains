using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	[ECompName ("province")]
	public class Province : EntityComponent, ISlotted<RegionSlot>
	{
		public override void CopyTo (GameObject go)
		{
			Province prov = go.AddComponent<Province> ();
			prov.tiles = this.tiles;
		}

		List<TileHandle> tiles;

		public void Receive (RegionSlot data)
		{
			tiles = data.Tiles;
		}

		public override void LoadFromTable (ITable table)
		{
		}

	}

}

