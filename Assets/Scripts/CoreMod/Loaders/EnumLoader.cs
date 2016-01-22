using UnityEngine;
using System.Collections;
using Demiurg.Core.Extensions;
using System;

namespace CoreMod
{
	public class EnumLoader : IConfigLoader
	{
		public bool IsSpecific ()
		{
			return  false;
		}

		public bool Check (System.Type targetType)
		{
			return targetType.IsEnum;
		}

		public object Load (object fromObject, System.Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			object enumeration = Activator.CreateInstance (targetType);
			try {

				enumeration = Enum.Parse (targetType, (string)fromObject);
			} catch {
				enumeration = Activator.CreateInstance (targetType);
			}
			return enumeration;
		}



	}
}

