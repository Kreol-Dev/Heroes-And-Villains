using UnityEngine;
using System.Collections;
using MapRoot;
using System.Collections.Generic;
using UIO;
using UnityEngine.UI;

namespace CoreMod
{
	public class RegionsLayer : MapLayer, ITileMapLayer<GameObject>, IListMapLayer<RegionObject>
	{
		[Defined ("go_layer")]
		string targetGoName;

		public MapHandle MapHandle { get; internal set; }

		public Signals.Signal<TileHandle> TileUpdated { get; internal set; }

		public Signals.Signal MassUpdate { get; internal set; }

		public GameObject[,] Tiles { get; internal set; }


		public event ObjectDelegate<RegionObject> ObjectAdded;
		public event ObjectDelegate<RegionObject> ObjectRemoved;

		Dictionary<Zone, RegionObjectTiles> zones = new Dictionary<Zone,  RegionObjectTiles> ();

		class RegionObjectTiles
		{
			public RegionObject Region;
			public HashSet<TileHandle> Tiles;
		}

		public bool AddObject (RegionObject go)
		{
			if (!zones.ContainsKey (go.Zone))
			{
				if (ObjectAdded != null)
					ObjectAdded (go);
				HashSet<TileHandle> tiles = new HashSet<TileHandle> (go.Tiles);
				zones.Add (go.Zone, new RegionObjectTiles (){ Region = go, Tiles = tiles });
				Set (go.Zone);
				go.Zone.ZoneUpdated += UpdateTiles;
				return true;
			}
			return false;
			
		}

		void UpdateTiles (Zone zone)
		{
			Clear (zone);
			var obj = zones [zone];
			obj.Tiles.Clear ();
			foreach (var tile in obj.Region.Tiles)
				obj.Tiles.Add (tile);
			Set (zone);
		}

		void Clear (Zone zone)
		{
			var obj = zones [zone];
			foreach (var tile in obj.Tiles)
				if (tile.Get (Tiles) == obj.Region.gameObject)
				{
					tile.Set (Tiles, null);
					TileUpdated.Dispatch (tile);
				}
		}

		void Set (Zone zone)
		{
			var obj = zones [zone];
			foreach (var tile in obj.Tiles)
				if (tile.Get (Tiles) == null)
				{
					tile.Set (Tiles, obj.Region.gameObject);
					TileUpdated.Dispatch (tile);
				}
		}

		public bool RemoveObject (RegionObject go)
		{
			if (zones.ContainsKey (go.Zone))
			{
				if (ObjectRemoved != null)
					ObjectRemoved (go);
				zones.Remove (go.Zone);
				go.Zone.ZoneUpdated -= UpdateTiles;
				return true;
			}
			return false;
		}

		public bool HasObject (RegionObject go)
		{
			return zones.ContainsKey (go.Zone);
		}

		protected override void Setup (ITable definesTable, MapRoot.Map mapRoot)
		{
			IListMapLayer<GameObject> goLayer = mapRoot.GetLayer (targetGoName) as IListMapLayer<GameObject>;
			goLayer.ObjectAdded += obj => {
				var regionCmp = obj.GetComponent<RegionObject> ();
				if (regionCmp != null)
					this.AddObject (regionCmp);
			};
			goLayer.ObjectRemoved += obj => {
				var regionCmp = obj.GetComponent<RegionObject> ();
				if (regionCmp != null)
					this.RemoveObject (regionCmp);
			};
			TileUpdated = new Signals.Signal<TileHandle> ();
			MassUpdate = new Signals.Signal ();
			var map = Find.Root<TilesRoot> ().MapHandle;
			MapHandle = map;
			Tiles = new GameObject[map.SizeX, map.SizeY];
			for (int i = 0; i < map.SizeX; i++)
				for (int j = 0; j < map.SizeY; j++)
				{

					Tiles [i, j] = null;

				}
			MassUpdate.Dispatch ();
		}

	}

	public class GOInteractor : TiledObjectsLayerInteractor<GameObject, RegionsLayer>
	{
		
	}







	public class GORenderer : BaseMapLayerRenderer<RegionsLayer, GOInteractor>
	{
		public override void ChangeState (RepresenterState state)
		{
		}

		protected override void Setup (ITable definesTable, ITable rendererData)
		{
			Debug.LogWarning ("SETUP GO RENDERER");
		}



	}



}
