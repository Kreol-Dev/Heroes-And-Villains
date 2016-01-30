using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System;

namespace CoreMod
{
	public class ColorLoader : IConfigLoader
	{
		Type colorType = typeof(Color);

		public bool IsSpecific ()
		{
			return true;
		}

		public bool Check (System.Type targetType)
		{
			return targetType == colorType;
		}

		IConfigLoader floatLoader = null;
		Type floatType = typeof(float);

		public object Load (ITable fromTable, object id, System.Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			ITable table = fromTable.GetTable (id) as ITable;
			if (table == null)
				return Color.clear;
			float red = table.GetFloat (1);
			float green = table.GetFloat (2);
			float blue = table.GetFloat (3);
			Color color = new Color (red, green, blue);
			Debug.LogFormat ("COLOR LOADED {0}", color);
			return color;
		}



	}
}



