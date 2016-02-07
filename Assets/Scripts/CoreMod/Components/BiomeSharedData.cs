using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	public class BiomeSharedData
	{
		public GraphicsTile graphicsTile;
		public int movementCost;
		public string biomeName;
		public ITileMapLayer<GraphicsTile> layer;
	}


}


