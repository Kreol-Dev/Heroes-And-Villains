using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using Demiurg.Core;

namespace CoreMod
{
	public class RandomPointsOnTiles : Demiurg.Core.Avatar
	{
		[AInput ("main")]
		List<TileHandle[]> mainI;
		[AOutput ("main")]
		TileHandle[] mainO;
		[AConfig ("density")]
		int density;
		[AConfig ("min_count")]
		int minCount;

		public override void Work ()
		{
			if (minCount < 0)
				minCount = 0;
			int pointsCount = 0;
			for (int i = 0; i < mainI.Count; i++)
				pointsCount += mainI [i].Length / density + minCount;
			TileHandle[] points = new TileHandle[pointsCount];
			int curPoint = 0;
			for (int i = 0; i < mainI.Count; i++)
			{
				TileHandle[] tiles = mainI [i];
				int localCount = tiles.Length / density + minCount;
				if (localCount == 0)
					continue;

				int chunkRange = tiles.Length / localCount;
				for (int j = 0, r = 0; j < localCount; j++, r += chunkRange)
				{
					points [curPoint++] = tiles [Random.Next (r, r + chunkRange)];
				}
			}
			mainO = points;
			FinishWork ();
		}
        
	}

}

