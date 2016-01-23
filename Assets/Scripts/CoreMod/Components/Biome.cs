using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	[ECompName ("biome")]
	public class Biome : EntityComponent<BiomeSharedData>, ISlotted<RegionSlot>
	{
		List<TileHandle> tiles;

		public void Receive (RegionSlot data)
		{
			tiles = data.Tiles;
			gameObject.AddComponent<TilesComponent> ().tiles = tiles;
			foreach (var tile in tiles)
			{
				tile.Set (SharedData.layer.Tiles, SharedData.graphicsTile);
				SharedData.layer.TileUpdated.Dispatch (tile, SharedData.graphicsTile);
			}
		}

		public override void LoadFromTable (ITable table)
		{
			SharedData = new BiomeSharedData ();
			string graphicsLayerName = (string)Find.Root<ModsManager> ().GetTable ("defines").Get ("BIOMES_GRAPHICS_LAYER");
			SharedData.layer = Find.Root<MapRoot.Map> ().GetLayer (graphicsLayerName) as ITileMapLayer<GraphicsTile>;
			SharedData.movementCost = (int)(double)table.Get ("tile_movement_cost");
			int priority = (int)(double)table.Get ("priority");
			string spriteName = (string)table.Get ("tile_graphics");
			SharedData.biomeName = (string)table.Get ("name");
			SharedData.graphicsTile = new GraphicsTile (Find.Root<Sprites> ().GetSprite ("map_tiles", spriteName), priority, SharedData.biomeName + " tile");
				

		}

		public override void CopyTo (GameObject go)
		{
			Biome biome = go.AddComponent<Biome> ();
			biome.SharedData = SharedData;
		}
	}




}


