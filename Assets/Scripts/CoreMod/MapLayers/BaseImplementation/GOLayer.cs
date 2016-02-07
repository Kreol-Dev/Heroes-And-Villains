using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	public class GOLayer : MapLayer, ITileMapLayer<GameObject>, IListMapLayer<GameObject>
	{
		public Signals.Signal<GameObject> ObjectAdded { get; internal set; }

		public Signals.Signal<GameObject> ObjectRemoved { get; internal set; }

		public Signals.Signal<GameObject> ObjectUpdated { get; internal set; }

		public MapHandle MapHandle { get; internal set; }

		public Signals.Signal<TileHandle, GameObject> TileUpdated { get; internal set; }

		public Signals.Signal MassUpdate { get; internal set; }

		public GameObject[,] Tiles { get; internal set; }

		public HashSet<GameObject> gos = new HashSet<GameObject> ();

		protected void OnObjectAdded (GameObject obj)
		{
			var cmp = obj.GetComponent<TilesComponent> ();
			if (cmp == null)
				return;
			var tiles = cmp.tiles;
			if (!gos.Add (obj))
				return;
			if (tiles == null)
				return;
			foreach (var tile in tiles)
				tile.Set (Tiles, obj);
			
		}

		protected void OnObjectRemoved (GameObject obj)
		{
			var cmp = obj.GetComponent<TilesComponent> ();
			if (cmp == null)
				return;
			var tiles = cmp.tiles;
			if (!gos.Remove (obj))
				return;
			if (tiles == null)
				return;
			foreach (var tile in tiles)
				tile.Set (Tiles, null);
		}

		protected void OnObjectUpdated (GameObject obj)
		{
			var cmp = obj.GetComponent<TilesComponent> ();
			if (cmp == null)
				return;
			var tiles = cmp.tiles;
			if (!gos.Add (obj))
				return;
			if (tiles == null)
				return;
			foreach (var tile in tiles)
				tile.Set (Tiles, obj);
		}

		protected override void Setup (ITable definesTable)
		{
			ObjectAdded = new Signals.Signal<GameObject> ();
			ObjectAdded.AddListener (OnObjectAdded);
			ObjectRemoved = new Signals.Signal<GameObject> ();
			ObjectRemoved.AddListener (OnObjectRemoved);
			ObjectUpdated = new Signals.Signal<GameObject> ();
			ObjectUpdated.AddListener (OnObjectUpdated);
			TileUpdated = new Signals.Signal<TileHandle, GameObject> ();
			MassUpdate = new Signals.Signal ();
			var map = Find.Root<TilesRoot> ().MapHandle;

			Tiles = new GameObject[map.SizeX, map.SizeY];
			for (int i = 0; i < map.SizeX; i++)
				for (int j = 0; j < map.SizeY; j++)
				{

					Tiles [i, j] = null;

				}
			MassUpdate.Dispatch ();
		}

	}

	public class GOInteractor : TiledObjectsLayerInteractor<GameObject, GameObject, GOLayer>
	{
		public override bool ObjectFromLayerObject (GameObject obj, out GameObject outObj)
		{
			if (obj != null)
				Debug.Log ("GO " + obj.name);
			outObj = obj;
			return obj != null;
		}

	}

	public class GORenderer : BaseMapLayerRenderer<GOLayer, GOInteractor>
	{
		public override void ChangeState (RepresenterState state)
		{
		}

		protected override void Setup (ITable definesTable)
		{
			Debug.LogWarning ("SETUP GO RENDERER");
		}



	}



}
