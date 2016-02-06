using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;
using System;

namespace MapRoot
{
	[RootDependencies (typeof(ModsManager))]
	public class Map : Root
	{
		Scribe scribe = Scribes.Find ("MAP");
		ModsManager mm = Find.Root<ModsManager> ();

		Dictionary<string, IMapCollection> collections = new Dictionary<string, IMapCollection> ();

		protected override void CustomSetup ()
		{
			ITable layersTable = mm.GetTable ("map_layers");
			foreach (var collectionKey in layersTable.GetKeys())
			{
				IMapCollection collection = ReadCollection (layersTable, collectionKey as string);
				if (collection == null)
					continue;
				collections.Add (collection.Name, collection);
			}
			Fulfill.Dispatch ();

		}

		IMapCollection ReadCollection (ITable table, string collectionName)
		{
			try
			{
				ITable colTable = table.GetTable ("collection");
				var collectionTypeName = table.GetString ("collection_type");
				Type type = mm.GetType (collectionTypeName);
				IMapCollection collection = Activator.CreateInstance (type) as IMapCollection;
				var layers = ReadLayers (table.GetTable ("layers"));
				foreach (var layer in layers)
					collection.AddLayer (layer, layer.Name);
				return collection;
			} catch (MissingFieldException e)
			{
				return null;
			}

		}

		List<IMapLayer> ReadLayers (ITable table)
		{
			List<IMapLayer> collectionLayers = new List<IMapLayer> ();
			foreach (var key in table.GetKeys())
			{
				string layerName = (string)key;
				string layerTypeName = table.GetString (layerName);
				Type type = Type.GetType (layerTypeName);
				IMapLayer layer = Activator.CreateInstance (type) as IMapLayer;
				if (layer == null)
				{
					scribe.LogFormatError ("Layer {0} is not a subclass of IMapLayer", layerName);
					continue;
				}
				layer.Setup (layerName);
				collectionLayers.Add (layer);

			}
			return collectionLayers;
		}

		protected override void PreSetup ()
		{
            
		}






		void RegisterMapCollection (string name, Type collectionType)
		{
			IMapCollection collection = Activator.CreateInstance (collectionType) as IMapCollection;
			if (collections.ContainsKey (name))
			{
				scribe.LogFormatError ("Map already contains collection with the name {0} and type {1} (duplicate has the type {2})", 
				                       name, collections [name].GetType (), collectionType);
				return;
			}
			collections.Add (name, collection);
		}

		public T GetCollection<T> (string name) where T : class, IMapCollection, new()
		{
			T collection;
			if (!collections.ContainsKey (name))
			{
				scribe.LogFormatWarning ("Map had no collection with the name {0} and type {1}, so it was added during GetCollectionr<T> call", name, typeof(T));
				RegisterMapCollection (name, typeof(T));
				return collections [name] as T;
			}
			collection = collections [name] as T;
			if (collection == null)
			{
				scribe.LogFormatError ("Map had a collection with the name {0} but the assumed type {1} was incorrect {2}, so it was returned during GetCollection call"
				                       , name, typeof(T), collections [name].GetType ());
				return new T ();
			}
			return collection;
		}

		public IMapCollection GetCollection (string name)
		{
			IMapCollection collection;
			if (!collections.ContainsKey (name))
			{
				scribe.LogFormatWarning ("Map had no collection with the name {0}, so null was returned", 
				                         name);
				return null;
			} else
				collection = collections [name];
			return collection;
		}

		public List<IMapCollection> GetAllCollections ()
		{
			List<IMapCollection> collectionsList = new List<IMapCollection> ();
			foreach (var collection in collections)
				collectionsList.Add (collection.Value);
			return collectionsList;
		}
	}

}

