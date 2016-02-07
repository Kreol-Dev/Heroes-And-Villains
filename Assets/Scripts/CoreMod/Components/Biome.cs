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
			string graphicsLayerName = Find.Root<ModsManager> ().GetTable ("defines").GetString ("BIOMES_GRAPHICS_LAYER");
			SharedData.layer = Find.Root<MapRoot.Map> ().GetLayer (graphicsLayerName) as ITileMapLayer<GraphicsTile>;
			SharedData.movementCost = table.GetInt ("tile_movement_cost");
			int priority = table.GetInt ("priority");
			string spriteName = table.GetString ("tile_graphics");
			SharedData.biomeName = table.GetString ("name");
			SharedData.graphicsTile = new GraphicsTile (Find.Root<Sprites> ().GetSprite ("map_tiles", spriteName), priority, SharedData.biomeName + " tile");
				

		}

		public override void CopyTo (GameObject go)
		{
			Biome biome = go.AddComponent<Biome> ();
			biome.SharedData = SharedData;
		}
	}




}


