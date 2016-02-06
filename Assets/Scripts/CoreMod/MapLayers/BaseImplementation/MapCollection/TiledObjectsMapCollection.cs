using UnityEngine;
using System.Collections;
using MapRoot;
using Demiurg.Core.Extensions;
using System.Collections.Generic;
using System;

namespace CoreMod
{
	public delegate void VoidDelegate ();
	public abstract class TiledObjectsMapCollection<TIdentifier> : BaseMapCollection where TIdentifier : class
	{
		HashSet<TIdentifier> objectIDs = new HashSet<TIdentifier> ();

		public IEnumerable<TIdentifier> GetAllIDs ()
		{
			return objectIDs;
		}

		public event TileDelegate TileChanged;

		public MapHandle MapHandle { get; private set; }

		protected abstract IEnumerable<TileHandle> GetTilesFromObject (TIdentifier id);

		TIdentifier[,] Environment;

		public TIdentifier GetObjectID (TileHandle handle)
		{
			return handle.Get (Environment);
		}

		public void AddObject (TIdentifier id)
		{
			
			if (objectIDs.Contains (id))
				return;
			var handles = GetTilesFromObject (id);
			foreach (var handle in handles)
			{
				handle.Set (Environment, id);
				TileChanged (handle);
			}
		}

		public void RemoveObject (TIdentifier id)
		{
			if (!objectIDs.Contains (id))
				return;
			var handles = GetTilesFromObject (id);
			foreach (var handle in handles)
			{
				handle.Set (Environment, null);
				TileChanged (handle);
			}
		}


		protected override void Setup (ITable definesTable)
		{

			MapHandle = Find.Root<TilesRoot> ().MapHandle;
			Environment = new TIdentifier[MapHandle.SizeX, MapHandle.SizeY];
			for (int i = 0; i < MapHandle.SizeX; i++)
				for (int j = 0; j < MapHandle.SizeY; j++)
					Environment [i, j] = null;
					
		}
		

	}


}
