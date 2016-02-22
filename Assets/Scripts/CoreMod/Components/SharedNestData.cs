using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	public class SharedNestData
	{
		public string MonsterName { get; internal set; }

		public ITileMapLayer<NestTile> NestsLayer { get; internal set; }

		public SharedNestData (string name, ITileMapLayer<NestTile> layer)
		{
			MonsterName = name;
			NestsLayer = layer;
		}
	}

}


