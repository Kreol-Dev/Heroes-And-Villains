using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;
using MapRoot;

namespace CoreMod
{
	[AShared]
	[ECompName ("monsters_nest")]
	public class MonstersNest : EntityComponent<SharedNestData>
	{
		List<NestTile> nests = new List<NestTile> ();

		public override void LoadFromTable (ITable table)
		{
			string monsterName = table.GetString ("monster_name", "NULL");

			Find.Root<ModsManager> ().Defs.LoadObjectAs<MonstersNest> (this, table);

			SharedData = new SharedNestData (monsterName, Find.Root<MapRoot.Map> ().GetLayer ("nests_tiles_layer") as ITileMapLayer<NestTile>);

		}

		public override EntityComponent CopyTo (GameObject go)
		{
			var nest = go.AddComponent<MonstersNest> ();
			nest.Count = this.Count;
			nest.Danger = this.Danger;
			nest.HasChief = this.HasChief;
			nest.Food = this.Food;
			nest.SharedData = this.SharedData;
			return nest;
//			nest.MonsterName = this.MonsterName;
		}

		public override void PostCreate ()
		{
			var tiles = GetComponent<TilesComponent> ();
			foreach (var tile in tiles.Tiles)
			{
				var nest = new NestTile (tile, SharedData.MonsterName);
				nest.Size = this.Count;
				nests.Add (nest);
				nest.Tile.Set (SharedData.NestsLayer.Tiles, nest);
				SharedData.NestsLayer.TileUpdated.Dispatch (nest.Tile);
				nest.Updated += NestTile_Updated;
			}
			StartCoroutine (NestSimulation ());
		}

		void NestTile_Updated (TileHandle tile)
		{
			SharedData.NestsLayer.TileUpdated.Dispatch (tile);
		}

		[Defined ("count")]
		public int Count;
		[Defined ("food")]
		public int Food;
		[Defined ("has_chief")]
		public bool HasChief;
		[Defined ("danger")]
		public int Danger;


		IEnumerator NestSimulation ()
		{
			
			while (this.enabled)
			{
				for (int i = 0; i < nests.Count; i++)
					nests [i].Size += Random.Range (-4, 4);
				yield return new WaitForSeconds (1f);
			}
		}

		protected override void PostDestroy ()
		{

		}

	}

	public class NestTile
	{
		public event TileDelegate Updated;

		int size;

		public int Size {
			get { return size; }
			set
			{
				size = value;
				if (Updated != null)
					Updated (Tile);
			}
		}

		public TileHandle Tile { get; internal set; }

		public string MonsterName { get; internal set; }

		public NestTile (TileHandle handle, string monsterName)
		{
			this.Tile = handle;
			this.MonsterName = monsterName;
		}

	}



	//	public class NestsTilesLayer : TilesLayer<NestTile>
	//	{
	//
	//	}

	//	public class NestsTilesRenderer : SpriteMapRenderer<NestTile, NestsTilesLayer>
	//	{
	//		Dictionary<string, GraphicsDeterminer> determiners = new Dictionary<string, GraphicsDeterminer> ();
	//
	//		public class GraphicsDeterminer
	//		{
	//			public Dictionary<int, GraphicsTile> tiles = new Dictionary<int, GraphicsTile> ();
	//
	//			public GraphicsTile DetermineTile (int count)
	//			{
	//				GraphicsTile tile = null;
	//				int max = int.MinValue;
	//				foreach (var pair in tiles)
	//				{
	//					if (pair.Key <= count && pair.Key > max)
	//					{
	//						max = pair.Key;
	//						tile = pair.Value;
	//					}
	//				}
	//				return tile;
	//			}
	//		}
	//
	//		protected override Sprite GetSprite (NestTile obj)
	//		{
	//			if (obj == null)
	//				return null;
	//			GraphicsDeterminer determiner = null;
	//			determiners.TryGetValue (obj.MonsterName, out determiner);
	//			if (determiner == null)
	//				return null;
	//			var tile = determiner.DetermineTile (obj.Size);
	//			if (tile == null)
	//				return null;
	//			return tile.Sprite;
	//		}
	//
	//		protected override void ReadRules (ITable rulesTable)
	//		{
	//			foreach (var monsterName in rulesTable.GetKeys())
	//			{
	//				ITable monsterRuleTable = rulesTable.GetTable (monsterName);
	//				GraphicsDeterminer determiner = new GraphicsDeterminer ();
	//				foreach (var spriteName in monsterRuleTable.GetKeys())
	//				{
	//					GraphicsTile tile = null;
	//					tiles.TryGetValue (spriteName as string, out tile);
	//					if (tile == null)
	//						continue;
	//					int count = monsterRuleTable.GetTable (spriteName).GetInt ("count");
	//					determiner.tiles.Add (count, tile);
	//				}
	//				determiners.Add (monsterName as string, determiner);
	//			}
	//		}
	//
	//	}
}


