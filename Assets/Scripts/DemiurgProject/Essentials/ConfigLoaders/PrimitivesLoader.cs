using Demiurg.Core.Extensions;
using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Demiurg.Essentials
{
	public class IntLoader : IConfigLoader
	{
		Type intType = typeof(int);

		public bool IsSpecific ()
		{
			return true;
		}

		public bool Check (Type targetType)
		{
			return targetType == intType;
		}

		public object Load (ITable fromTable, object id, Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			return fromTable.GetInt (id);
		}
	}

	public class FloatLoader : IConfigLoader
	{
		Type type = typeof(float);

		public bool IsSpecific ()
		{
			return true;
		}

		public bool Check (Type targetType)
		{
			return targetType == type;
		}

		public object Load (ITable fromTable, object id, Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			return fromTable.GetFloat (id);
		}
	}

	public class DoubleLoader : IConfigLoader
	{
		Type type = typeof(double);

		public bool IsSpecific ()
		{
			return true;
		}

		public bool Check (Type targetType)
		{
			return targetType == type;
		}

		public object Load (ITable fromTable, object id, Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			return fromTable.GetDouble (id);
		}
	}

	public class BoolLoader : IConfigLoader
	{
		Type type = typeof(bool);

		public bool IsSpecific ()
		{
			return true;
		}

		public bool Check (Type targetType)
		{
			return targetType == type;
		}

		public object Load (ITable fromTable, object id, Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			return fromTable.GetBool (id);
		}
	}

	public class StringLoader : IConfigLoader
	{
		Type type = typeof(string);

		public bool IsSpecific ()
		{
			return true;
		}

		public bool Check (Type targetType)
		{
			return targetType == type;
		}

		public object Load (ITable fromTable, object id, Type targetType, Demiurg.Core.ConfigLoaders loaders)
		{
			return fromTable.GetString (id);
		}
	}

}

