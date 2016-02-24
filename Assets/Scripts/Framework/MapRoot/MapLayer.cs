using UnityEngine;
using System.Collections;
using UIO;

namespace MapRoot
{
	public interface IMapLayer
	{
		string Name { get; }

		void Setup (string name);
	}

	public abstract class MapLayer : IMapLayer
	{
		public string Name { get; internal set; }

		public void Setup (string name)
		{
			this.Name = name;
			ITable table = Find.Root<ModsManager> ().GetTable ("defines");
			if (table != null)
				this.Setup (table);
		}

		protected abstract void Setup (ITable definesTable);
	}

	public class DefaultMapLayer : MapLayer
	{
		protected override void Setup (ITable definesTable)
		{
            
		}
	}
}


