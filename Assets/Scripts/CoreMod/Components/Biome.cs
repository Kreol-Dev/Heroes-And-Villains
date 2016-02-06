using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	[ECompName ("biome")]
	public class Biome : EntityComponent
	{
		public BiomeSharedData SharedData { get; private set; }

		List<TileHandle> tiles;

		const string collectionName = "biomes_tiled_collection";
		const string layerName = "biomes_layer";

		public override void LoadFromTable (ITable table)
		{
			SharedData = new BiomeSharedData ();
			var layCo = gameObject.AddComponent<LayerCollectionComponent> ();
			layCo.Collection = collectionName;
			layCo.Layer = layerName;
			//gameObject.AddComponent
//			SharedData.layer = Find.Root<MapRoot.Map> ().GetLayer (graphicsLayerName) as ITileMapLayer<GraphicsTile>;
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
			TilesComponent tilesCmp = GetComponent<TilesComponent> ();
			tiles = new List<TileHandle> (tilesCmp.Tiles);
		}
	}

	public class TiledBiomesLayer : GOLayer<Biome>
	{
		
	}


}


