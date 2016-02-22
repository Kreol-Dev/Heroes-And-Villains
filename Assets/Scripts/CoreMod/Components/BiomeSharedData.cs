using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	public class BiomeSharedData
	{
		public BiomeTile BiomeTile { get; internal set; }

		public ITileMapLayer<BiomeTile> Layer { get; internal set; }

		public BiomeSharedData (ITileMapLayer<BiomeTile> layer, BiomeTile biomeTile)
		{
			this.BiomeTile = biomeTile;
			this.Layer = layer;
		}
	}


}


