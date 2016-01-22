using UnityEngine;
using System.Collections;
using System;
using Demiurg.Core.Extensions;

namespace Demiurg.Essentials
{
	public class ICallbackLoader : IConfigLoader
	{
		Type type = typeof(ICallback);

		public bool IsSpecific ()
		{
			return true;
		}

		public bool Check (Type targetType)
		{
			return type.IsAssignableFrom (targetType);
		}

		public object Load (object fromObject, Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			return fromObject as ICallback;

		}

	}
}