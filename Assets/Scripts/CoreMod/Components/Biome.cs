using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapRoot;
using UIO;

namespace CoreMod
{
	[AShared]
	[ECompName ("biome")]
	public class Biome : EntityComponent<BiomeSharedData>
	{
		static int tileID = 0;
		[Defined ("movement_cost")]
		public int MovementCost;
		[Defined ("name")]
		public string Name;

		public override void LoadFromTable (ITable table)
		{
			MovementCost = table.GetInt ("tile_movement_cost");
			Name = table.GetString ("name");
			SharedData = new BiomeSharedData (table.GetString ("biome_type"));
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


		}
	}




	public class BiomesRenderer : SpriteMapRenderer<GameObject, RegionsLayer>
	{
		protected override Sprite GetSprite (GameObject obj)
		{
			if (obj == null)
				return null;
			var biomeCmp = obj.GetComponent<Biome> ();
			if (biomeCmp == null)
				return null;

			GraphicsTile tile = null;
			BiomeTiles.TryGetValue (biomeCmp.SharedData, out tile);
			if (tile == null)
			{
				string tileName = null;
				if (!typeToTile.TryGetValue (biomeCmp.SharedData.BiomeType, out tileName))
					return null;
				if (!tiles.TryGetValue (tileName, out tile))
					return null;
				BiomeTiles.Add (biomeCmp.SharedData, tile);
			}
			return tile.Sprite;
		}

		Dictionary<BiomeSharedData, GraphicsTile> BiomeTiles = new Dictionary<BiomeSharedData, GraphicsTile> ();

		[Defined ("type_to_tile")]
		Dictionary<string, string> typeToTile = new Dictionary<string, string> ();

		protected override void ReadRules (ITable rulesTable)
		{
			Debug.Log ("-------------------------------------------------------------");
			Find.Root<ModsManager> ().Defs.LoadObjectAs<BiomesRenderer> (this, rulesTable);

			Debug.Log ("-------------------------11111111111111-----------------------");
			foreach (var typeTilePair in typeToTile)
				Debug.LogFormat ("{0} {1}", typeTilePair.Key, typeTilePair.Value);

		}


		protected override void Clicked (GameObject go)
		{
		}

		protected override void DeClicked (GameObject go)
		{
		}

		protected override void Hovered (GameObject go)
		{
		}

		protected override void DeHovered (GameObject go)
		{
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


