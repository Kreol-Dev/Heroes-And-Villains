using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	public class GOLayer : MapLayer, ITileMapLayer<GameObject>, IListMapLayer<GameObject>
	{

		public MapHandle MapHandle { get; internal set; }

		public Signals.Signal<TileHandle> TileUpdated { get; internal set; }

		public Signals.Signal MassUpdate { get; internal set; }

		public GameObject[,] Tiles { get; internal set; }


		public event ObjectDelegate<GameObject> ObjectAdded;
		public event ObjectDelegate<GameObject> ObjectRemoved;

		HashSet<GameObject> gos = new HashSet<GameObject> ();

		public bool AddObject (GameObject go)
		{
			if (gos.Add (go))
			{
				if (ObjectAdded != null)
					ObjectAdded (go);
				return true;
			}
			return false;
			
		}

		public bool RemoveObject (GameObject go)
		{
			if (gos.Remove (go))
			{
				if (ObjectRemoved != null)
					ObjectRemoved (go);
				return true;
			}
			return false;
		}

		protected override void Setup (ITable definesTable)
		{
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

	public class GOInteractor : TiledObjectsLayerInteractor<GameObject, GOLayer>
	{
		
	}

	public class GORenderer : BaseMapLayerRenderer<GOLayer, GOInteractor>
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
