using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;
using System;

namespace MapRoot
{
	[RootDependencies (typeof(ModsManager))]
	public class Map : Root
	{
		Scribe scribe;
		Dictionary<string, IMapLayer> layers = new Dictionary<string, IMapLayer> ();

		protected override void CustomSetup ()
		{

			ITable layersTable = Find.Root<ModsManager> ().GetTable ("map_layers");

			var mm = Find.Root<ModsManager> ();
			foreach (var key in layersTable.GetKeys())
			{
				try
				{
					string layerName = (string)key;
					ITable layerTable = layersTable.GetTable (key);

					if (mm.IsTechnical (layersTable, key))
						continue;
					string layerTypeName = layerTable.GetString ("layer_type");
					Type type = Type.GetType (layerTypeName);
					IMapLayer layer = Activator.CreateInstance (type) as IMapLayer;
					if (layer == null)
					{
						scribe.LogFormatError ("Layer {0} is not a subclass of IMapLayer", layerName);
						continue;
					}

					layers.Add (layerName, layer);
				} catch (ITableTypesMismatch e)
				{
					
				}

			}

			foreach (var layerPair in layers)
			{
				mm.Defs.LoadObjectAs<IMapLayer> (layerPair.Value, layersTable.GetTable (layerPair.Key).GetTable ("configs"));
				layerPair.Value.Setup (layerPair.Key, this);
			}
			Fulfill.Dispatch ();

		}

		protected override void PreSetup ()
		{
			scribe = Scribes.Find ("MAP");
		}


		public void RegisterMapLayer (string name, Type layerType)
		{
			IMapLayer layer = Activator.CreateInstance (layerType) as IMapLayer;
			if (layers.ContainsKey (name))
			{
				scribe.LogFormatError ("Map already contains layer with the name {0} and type {1} (duplicate has the type {2})", 
				                       name, layers [name].GetType (), layerType);
				return;
			}
			layers.Add (name, layer);
		}

		public T GetLayer<T> (string name) where T : class, IMapLayer, new()
		{
			T layer;
			if (!layers.ContainsKey (name))
			{
				scribe.LogFormatWarning ("Map had no layer with the name {0} and type {1}, so it was added during GetLayer<T> call", name, typeof(T));
				RegisterMapLayer (name, typeof(T));
				return layers [name] as T;
			}
			layer = layers [name] as T;
			if (layer == null)
			{
				scribe.LogFormatError ("Map had a layer with the name {0} but the assumed type {1} was incorrect {2}, so it was returned during GetLayer call"
                    , name, typeof(T), layers [name].GetType ());
				return new T ();
			}
			return layer;
		}

		public IMapLayer GetLayer (string name)
		{
			IMapLayer layer;
			if (!layers.ContainsKey (name))
			{
				scribe.LogFormatWarning ("Map had no layer with the name {0}, so DefaultMapLayer was added during GetLayer call", 
				                         name);
				layer = new DefaultMapLayer ();
				layer.Setup (name, this);
			} else
				layer = layers [name];
			return layer;
		}

		public string[] GetAllLayerNames ()
		{
			List<string> names = new List<string> ();
			foreach (var layer in layers)
				names.Add (layer.Key);
			return names.ToArray ();
		}
	}

}

