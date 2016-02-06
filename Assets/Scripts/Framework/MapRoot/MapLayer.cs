using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;

namespace MapRoot
{
	public interface IMapLayer
	{
		string Name { get; }

		void Setup (string name);

		void AttachToACollection (IMapCollection collection);
	}

	public abstract class MapLayer<TCollection> : IMapLayer where TCollection : class, IMapCollection
	{
		Scribe scribe = Scribes.Find ("LAYERS");

		public string Name { get; internal set; }

		protected TCollection Collection { get; private set; }

		public void AttachToACollection (IMapCollection collection)
		{
			if (!typeof(TCollection).IsAssignableFrom (collection.GetType ()))
			{
				scribe.LogFormatError ("Can't add collection {0} ({1}) to layer {2}({3}) as the layer supports collections only derived from {4}",
				                       collection.Name, collection.GetType (), this.Name, this.GetType (), typeof(TCollection));
				return;
			}
			Collection = collection as TCollection;
		}

		public void Setup (string name)
		{
			this.Name = name;
			ITable table = Find.Root<ModsManager> ().GetTable ("defines");
			if (table != null)
				this.Setup (table);
		}

		protected abstract void Setup (ITable definesTable);
	}


	public class DefaultMapLayer : MapLayer<IMapCollection>
	{
		protected override void Setup (ITable definesTable)
		{
            
		}
	}
}


