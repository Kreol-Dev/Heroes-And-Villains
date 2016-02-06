using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapRoot;

namespace CoreMod
{
	public class GOCollection : TiledObjectsMapCollection<GameObject>
	{
		protected override IEnumerable<TileHandle> GetTilesFromObject (GameObject id)
		{
			return id.GetComponent<TilesComponent> ().Tiles;
		}
	}


	public abstract class GOLayer<TObject> : MapLayer<GOCollection>, ITiledMapLayer<TObject, GameObject> where TObject : EntityComponent
	{
		public event TileDelegate TileChanged;

		public TObject GetObject (GameObject id)
		{
			return FromObject (id);
		}


		public event ObjectDelegate<TObject> ObjectChanged;

		public int MapSizeX { get { return Collection.MapHandle.SizeX; } }

		public int MapSizeY { get { return Collection.MapHandle.SizeY; } }

		public TObject GetObject (TileHandle handle)
		{
			return handle.Get (Tiles);
		}

		TObject[,] Tiles { get; set; }

		protected TObject FromObject (GameObject go)
		{
			return go == null ? null : go.GetComponent<TObject> ();

		}

		protected override void Setup (Demiurg.Core.Extensions.ITable definesTable)
		{
			Tiles = new TObject[Collection.MapHandle.SizeX, Collection.MapHandle.SizeY];
			for (int i = 0; i < Collection.MapHandle.SizeX; i++)
				for (int j = 0; j < Collection.MapHandle.SizeY; j++)
					Tiles [i, j] = null;


			Collection.TileChanged += OnTileChange;
		}

		void OnTileChange (TileHandle tile)
		{
			TObject obj = FromObject (Collection.GetObjectID (tile));
			TileChanged (tile);
			tile.Set (Tiles, obj);
			if (obj != null)
			{
				obj.NotifyUpdate -= OnNotifyUpdate;
				obj.NotifyUpdate += OnNotifyUpdate;
			}
		}

		void OnNotifyUpdate (EntityComponent cmp)
		{
			ObjectChanged (cmp as TObject);
		}

		List<TileHandle> cachedList = new List<TileHandle> ();

		public IEnumerable<TileHandle> TilesOfObjects (TObject obj)
		{
			return obj.gameObject.GetComponent<TilesComponent> ().Tiles;
		}
		
	}

	public class DefaultGOLayer : MapLayer<GOCollection>, ITiledMapLayer<GameObject, GameObject>
	{
		protected override void Setup (Demiurg.Core.Extensions.ITable definesTable)
		{
			Collection.TileChanged += TileChanged.Invoke;
		}

		public event ObjectDelegate<GameObject> ObjectChanged;

		public event TileDelegate TileChanged;

		public GameObject GetObject (TileHandle handle)
		{
			return Collection.GetObjectID (handle);
		}

		public GameObject GetObject (GameObject id)
		{
			return id;
		}

		public int MapSizeX { get { return Collection.MapHandle.SizeX; } }

		public int MapSizeY { get { return Collection.MapHandle.SizeY; } }
	}
		

}
