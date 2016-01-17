using UnityEngine;
using System.Collections;
using Demiurg;
using Demiurg.Core;

namespace CoreMod
{
	public class ClimateDataGatherer : SlotsProcessor
	{
		[AInput ("inlandness_map")]
		float[,] inlandnessMap;
		[AInput ("temperature_map")]
		int[,] temperatureMap;
		[AInput ("height_map")]
		int[,] heightMap;

		public override void Work ()
		{
			foreach (var go in InputObjects) {
				SlotTile tile = go.GetComponent<SlotTile> ();
				SlotClimate climate = go.AddComponent<SlotClimate> ();
				climate.Height = heightMap [tile.X, tile.Y];
				climate.Temperature = temperatureMap [tile.X, tile.Y];
				climate.Inlandness = inlandnessMap [tile.X, tile.Y];
				climate.Humidity = 0;
			}
			OutputObjects = InputObjects;
			FinishWork ();
		}
	}
}


