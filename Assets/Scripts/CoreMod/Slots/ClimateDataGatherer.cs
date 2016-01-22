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
		float[,] temperatureMap;
		[AInput ("humidity_map")]
		float[,] humidityMap;
		[AInput ("height_map")]
		float[,] heightMap;
		[AInput ("radiation_map")]
		float[,] radiationMap;

		public override void Work ()
		{
			foreach (var go in InputObjects)
			{
				SlotClimate climate = go.AddComponent<SlotClimate> ();
				SlotTile tile = go.GetComponent<SlotTile> ();
				RegionSlot region = go.GetComponent<RegionSlot> ();
				if (tile != null)
				{
					climate.Height = heightMap [tile.X, tile.Y];
					climate.Temperature = temperatureMap [tile.X, tile.Y];
					climate.Inlandness = inlandnessMap [tile.X, tile.Y];
					climate.Humidity = humidityMap [tile.X, tile.Y];
					climate.Radioactivity = radiationMap [tile.X, tile.Y];
				} else if (region != null)
				{
					foreach (var handle in region.Tiles)
					{
						climate.Height += handle.Get (heightMap);
						climate.Temperature += handle.Get (temperatureMap);
						climate.Inlandness += handle.Get (inlandnessMap);
						climate.Humidity += handle.Get (humidityMap);
						climate.Radioactivity += handle.Get (radiationMap);
					}

					climate.Height /= region.Tiles.Count;
					climate.Temperature /= region.Tiles.Count;
					climate.Inlandness /= region.Tiles.Count;
					climate.Humidity /= region.Tiles.Count;
					climate.Radioactivity /= region.Tiles.Count;
				}



			}
			OutputObjects = InputObjects;
			FinishWork ();
		}
	}
}


