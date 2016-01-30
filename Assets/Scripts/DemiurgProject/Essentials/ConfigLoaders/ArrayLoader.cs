using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;
using System;
using MoonSharp.Interpreter;

namespace Demiurg.Essentials
{
	public class ArrayLoader : IConfigLoader
	{

		public bool IsSpecific ()
		{
			return false;
		}

		public bool Check (Type targetType)
		{
			return targetType.IsArray;
		}

		public object Load (ITable fromTable, object id, Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			List<object> objects = new List<object> ();
			ITable table = fromTable.GetTable (id);
			Type containedType = targetType.GetGenericArguments () [0];
			IConfigLoader loader = loaders.FindLoader (containedType);
			var keys = table.GetKeys ();
			foreach (var key in keys)
			{
				objects.Add (loader.Load (table, key, containedType, loaders));
			}
			return objects.ToArray ();

		}


	}
}
