using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapRoot;
using UIO;
using UnityEngine.UI;

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
			this.Name = this.Name + " of " + Find.Root<NamesRoot> ().GenerateBiomeName ();

		}

		protected override void PostDestroy ()
		{
			
		}
	}


	public class Climate : EntityComponent
	{
		
		public float Height;
		public float Temperature;
		public float Humidity;
		public float Radioactity;

		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<Climate> ();
		}

		public override void PostCreate ()
		{
			
		}

		protected override void PostDestroy ()
		{
			
		}
	}

	public class Diseases : EntityComponent
	{
		
		public override EntityComponent CopyTo (GameObject go)
		{
			return go.AddComponent<Diseases> ();
		}

		public override void PostCreate ()
		{

		}

		protected override void PostDestroy ()
		{

		}
	}

	public class Disease
	{
		
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
			Find.Root<ModsManager> ().Defs.LoadObjectAs<BiomesRenderer> (this, rulesTable);
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

	public class BiomePresenter : ObjectPresenter<GameObject>
	{
		Text selectionText;
		Text hoverText;

		public override void Setup (ITable definesTable)
		{
			selectionText = (Object.Instantiate (Resources.Load ("UI/Text")) as GameObject).GetComponent<Text> ();
			var selectionLayout = selectionText.gameObject.AddComponent<LayoutElement> ();
			selectionLayout.minHeight = 20;
			selectionLayout.minWidth = 200;
			hoverText = (Object.Instantiate (Resources.Load ("UI/Text")) as GameObject).GetComponent<Text> ();
			var hoverLayout = hoverText.gameObject.AddComponent<LayoutElement> ();
			hoverLayout.minHeight = 20;
			hoverLayout.minWidth = 200;
			hoverText.text = "";
			hoverText.gameObject.SetActive (false);
			selectionText.text = "";
			selectionText.gameObject.SetActive (false);
			Transform selectionGO = GameObject.Find ("SelectionPanel").transform;
			Transform hoverGO = GameObject.Find ("HoverPanel").transform;
			selectionText.transform.SetParent (selectionGO);
			hoverText.transform.SetParent (hoverGO);
		}

		public override void ShowObjectDesc (GameObject obj)
		{
			selectionText.gameObject.SetActive (true);
			selectionText.text = obj.GetComponent<Biome> ().Name;
		}

		public override void HideObjectDesc ()
		{
			selectionText.gameObject.SetActive (false);
		}

		public override void ShowObjectShortDesc (GameObject obj)
		{
			hoverText.gameObject.SetActive (true);
			hoverText.text = obj.GetComponent<Biome> ().Name;
		}

		public override void HideObjectShortDesc ()
		{
			hoverText.gameObject.SetActive (false);
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


