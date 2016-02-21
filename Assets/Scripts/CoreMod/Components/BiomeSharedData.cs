using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	[AShared]
	public class BiomeSharedData
	{
		public GraphicsTile graphicsTile;
		public ITileMapLayer<GraphicsTile> layer;
	}


}


