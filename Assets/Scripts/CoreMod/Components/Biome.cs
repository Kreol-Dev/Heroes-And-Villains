using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	[ECompName ("biome")]
	public class Biome : EntityComponent<BiomeSharedData>
	{
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


		public override void PostCreate ()
		{
			foreach (var handle in gameObject.GetComponent<TilesComponent> ().Tiles)
			{
				handle.Set (SharedData.layer.Tiles, SharedData.graphicsTile);
				SharedData.layer.TileUpdated.Dispatch (handle);
			}

		}

		void OnTileAdded (TileHandle handle)
		{
			handle.Set (SharedData.layer.Tiles, SharedData.graphicsTile);
			SharedData.layer.TileUpdated.Dispatch (handle);
		}

		void OnTileRemoved (TileHandle handle)
		{
			handle.Set (SharedData.layer.Tiles, null);
			SharedData.layer.TileUpdated.Dispatch (handle);
		}
	}




}


