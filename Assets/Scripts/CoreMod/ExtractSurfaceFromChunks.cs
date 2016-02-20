using UnityEngine;
using System.Collections;
using Demiurg;
using System.Collections.Generic;
using Demiurg.Core;

namespace CoreMod
{
	public class ExtractSurfaceFromChunks  : Demiurg.Core.Avatar
	{
		[AInput ("main")]
		List<GameObject> mainI;
		[AOutput ("main")]
		List<TileHandle[]> mainO;
		[AOutput ("extracted_chunks")]
		List<GameObject> gos;
		[AConfig ("target_surface")]
		int targetSurface;
		[AConfig ("filter_less")]
		int filterLess;

		public override void Work ()
		{
			gos = new List<GameObject> ();
			mainO = new List<TileHandle[]> ();
			for (int i = 0; i < mainI.Count; i++)
			{
				var input = mainI [i];
				ChunkSlot chunk = input.GetComponent<ChunkSlot> ();
				if (chunk.Surface == targetSurface)
				{
					try
					{

						input.GetComponent<SlotSurface> ().SurfaceID = targetSurface;
					
					} catch
					{
						input.AddComponent<SlotSurface> ().SurfaceID = targetSurface;
					}
					mainO.Add (chunk.Tiles);
					gos.Add (input);
				}
			}
			int k = 0;
			int count = mainO.Count;
			while (k < count)
			{
				if (mainO [k].Length < filterLess)
				{
					mainO.RemoveAt (k);
					gos.RemoveAt (k);
					count--;
				} else
					k++;
			}

			FinishWork ();
		}
    
	}
}

