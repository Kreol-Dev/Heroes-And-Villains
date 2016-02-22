using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using MapRoot;

namespace CoreMod
{
	[AShared]
	[ECompName ("biome")]
	public class Biome : EntityComponent<BiomeSharedData>
	{
		static int tileID = 0;
		public int MovementCost;
		public string Name;

		public override void LoadFromTable (ITable table)
		{
			MovementCost = table.GetInt ("tile_movement_cost");
			Name = table.GetString ("name");
			var layer = Find.Root<MapRoot.Map> ().GetLayer ("biomes_tiles_layer") as ITileMapLayer<BiomeTile>;
			var biomeTile = new BiomeTile (tileID++, this.gameObject.name);

			SharedData = new BiomeSharedData (layer, biomeTile);
		}

		public override EntityComponent CopyTo (GameObject go)
		{
			Biome biome = go.AddComponent<Biome> ();
			biome.SharedData = SharedData;
			biome.MovementCost = this.MovementCost;
			biome.Name = this.Name;
			return biome;
		}


		public override void PostCreate ()
		{
			var tilesCmp = gameObject.GetComponent<TilesComponent> ();
			tilesCmp.TileAdded += OnTileAdded;
			tilesCmp.TileRemoved += OnTileRemoved;
			foreach (var handle in tilesCmp.Tiles)
			{
				handle.Set (SharedData.Layer.Tiles, SharedData.BiomeTile);
				SharedData.Layer.TileUpdated.Dispatch (handle);
			}

		}

		void OnTileAdded (TileHandle handle)
		{
			handle.Set (SharedData.Layer.Tiles, SharedData.BiomeTile);
			SharedData.Layer.TileUpdated.Dispatch (handle);
		}

		void OnTileRemoved (TileHandle handle)
		{
			handle.Set (SharedData.Layer.Tiles, null);
			SharedData.Layer.TileUpdated.Dispatch (handle);
		}
	}

	public class BiomeTile
	{
		public int ID { get; internal set; }

		public string Name { get; internal set; }

		public BiomeTile (int id, string name)
		{
			ID = id;
			Name = name;
		}
	}


	public class MapBiomesTilesLayer : TilesLayer<BiomeTile>
	{

	}


	public class BiomesRenderer : SpriteMapRenderer<BiomeTile, MapBiomesTilesLayer>
	{
		Scribe scribe;
		Dictionary<BiomeTile, GraphicsTile> tilesByID = new Dictionary<BiomeTile, GraphicsTile> ();
		Dictionary<string, string> biomeToGraphics = new Dictionary<string, string> ();

		protected override Sprite GetSprite (BiomeTile obj)
		{
			scribe = Scribes.Find ("BIOMES RENDERER");
			GraphicsTile tile = null;
			tilesByID.TryGetValue (obj, out tile);
			if (tile == null)
			{
				string graphicsTileName = null;
				biomeToGraphics.TryGetValue (obj.Name, out graphicsTileName);
				if (graphicsTileName != null)
				{
					tiles.TryGetValue (graphicsTileName, out tile);
				}
				if (tile != null)
				{
					scribe.LogFormatWarning ("Registered biomeTile {0} with graphiсs {1}", obj.Name, tile.Name);
					tilesByID.Add (obj, tile);
				}
			}
			if (tile == null)
				return null;
			return tile.Sprite;
		}

		protected override void ReadRules (ITable rulesTable)
		{
			foreach (var biomeName in rulesTable.GetKeys())
			{
				biomeToGraphics.Add (biomeName as string, rulesTable.GetString (biomeName));
			}
		}
		
	}


	//	public class BiomeLayerPresentor : BaseMapLayerPresenter<BiomeTile, MapBiomesTilesLayer, TileMapLayerInteractor>
	//	{
	//		public override void ChangeState (RepresenterState state)
	//		{
	//
	//		}
	//
	//	}
	//
	//	public class BiomeTilePresenter : ObjectPresenter<BiomeTile>
	//	{
	//		public override void Setup (ITable definesTable)
	//		{
	//		}
	//
	//		public override void ShowObjectDesc (BiomeTile obj)
	//		{
	//		}
	//
	//		public override void HideObjectDesc ()
	//		{
	//		}
	//
	//		public override void ShowObjectShortDesc (BiomeTile obj)
	//		{
	//		}
	//
	//		public override void HideObjectShortDesc ()
	//		{
	//		}
	//
	//	}
}


