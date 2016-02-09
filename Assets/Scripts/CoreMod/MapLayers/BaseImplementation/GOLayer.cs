using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System.Collections.Generic;

namespace CoreMod
{
	public class GOLayer : MapLayer, ITileMapLayer<GameObject>
	{

		public MapHandle MapHandle { get; internal set; }

		public Signals.Signal<TileHandle> TileUpdated { get; internal set; }

		public Signals.Signal MassUpdate { get; internal set; }

		public GameObject[,] Tiles { get; internal set; }

		public HashSet<GameObject> gos = new HashSet<GameObject> ();


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

		protected override void Setup (ITable definesTable)
		{
			Debug.LogWarning ("SETUP GO RENDERER");
		}



	}



}
