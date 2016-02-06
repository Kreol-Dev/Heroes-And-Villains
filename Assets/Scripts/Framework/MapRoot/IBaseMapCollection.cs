using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Demiurg.Core.Extensions;

namespace MapRoot
{
	public interface IMapCollection
	{
		string Name { get; }

		void Setup (string name);

		void AddLayer (IMapLayer layer, string name);

		List<IMapLayer> GetAllLayers ();

		IMapLayer GetLayer (string name);
	}

	public abstract class BaseMapCollection : IMapCollection
	{
		
		Dictionary<string, IMapLayer> layers = new Dictionary<string, IMapLayer> ();
		Scribe scribe = Scribes.Find ("COLLECTIONS");

		public void Setup (string name)
		{
			Name = name;
			Setup (Find.Root<ModsManager> ().GetTable ("defines"));
		}

		protected abstract void Setup (ITable definesTable);

		public string Name { get; internal set; }

		public void AddLayer (IMapLayer layer, string name)
		{


			if (layers.ContainsKey (name))
			{
				scribe.LogFormatError ("Can't add layer {0} ({1}) to the collection {2}({3}) as collection already has a layer with this name and type {4}",
				                       name, layer.GetType (), this.Name, this.GetType (), layers [name].GetType ());
				return;
			}

			layer.AttachToACollection (this);
			layers.Add (name, layer);

		}

		public IMapLayer GetLayer (string name)
		{
			if (!layers.ContainsKey (name))
			{
				scribe.LogFormatError ("Can't get layer {0} from collection {1}({2}) as collection doesnt have it",
				                       name, this.Name, this.GetType ());
				return null;
			}

			return layers [name];
		}

		public T GetLayer<T> (string name) where T : class, IMapLayer, new()
		{
			if (!layers.ContainsKey (name))
			{
				scribe.LogFormatError ("Can't get layer {0} ({1}) from collection {2}({3}) as collection doesnt have it",
				                       name, typeof(T), this.Name, this.GetType ());
				return new T ();
			}

			T layer = layers [name] as T;
			if (layer == null)
			{
				scribe.LogFormatError ("Can't get layer {0} ({1}) from collection {2}({3}) as collection has a layer of incompatible type {4}",
				                       name, typeof(T), this.Name, this.GetType (), layers [name].GetType ());
				return new T ();
			}

			return layer;
		}



		public List<IMapLayer> GetAllLayers ()
		{
			List<IMapLayer> names = new List<IMapLayer> ();
			foreach (var layer in layers)
				names.Add (layer.Value);
			return names;
		}

	}
}


