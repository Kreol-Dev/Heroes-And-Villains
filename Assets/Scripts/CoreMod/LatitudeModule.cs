using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using Demiurg.Core;


namespace CoreMod
{
	public class LatitudeModule : Demiurg.Core.Avatar
	{
		[AOutput ("main")]
		float[,] mainO;

		public override void Work ()
		{
			int width = Find.Root<TilesRoot> ().MapHandle.SizeX;
			int height = Find.Root<TilesRoot> ().MapHandle.SizeY;
			float[,] latitudeMap = new float[width, height];
			float halfHeight = height / 2;
			for (int j = 0; j < height; j++)
			{
				float value = Mathf.Lerp (0f, 1f, Mathf.Abs (halfHeight - j) / halfHeight);
				for (int i = 0; i < width; i++)
					latitudeMap [i, j] = value;

			}
			mainO = latitudeMap;
			FinishWork ();
		}
        
        
	}
}
