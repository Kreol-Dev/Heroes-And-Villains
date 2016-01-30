using UnityEngine;
using System.Collections;
using System;

namespace Demiurg.Core.Extensions
{
	public interface IConfigLoader
	{
		bool IsSpecific ();

		bool Check (Type targetType);

		object Load (ITable fromTable, object id, Type targetType, ConfigLoaders loaders);
	}
}


