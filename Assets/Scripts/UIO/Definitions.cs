using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UIO.BasicConverters;

namespace UIO
{
	public class Definitions
	{
		Converters converters;
		Dictionary<string, Type> types = new Dictionary<string, Type> ();
		Scribe scribe;

		public Definitions (Converters converters, Dictionary<string, Type> types)
		{
			scribe = Scribes.Find ("DEFINITIONS");
			this.converters = converters;
			this.types = types;
		}

		Dictionary<Type, ObjectDefinition> defs = new Dictionary<Type, ObjectDefinition> ();

		void LoadObject (object obj, Type type, ITable table)
		{
			ObjectDefinition definition = null;
			if (!defs.TryGetValue (type, out definition))
			{
				definition = new ObjectDefinition (type, converters);
				scribe.LogFormatWarning ("{0}    |    {1}", type, definition);
				defs.Add (type, definition);
			}
			definition.LoadObject (obj, table);

		}

		public Type GetObjectType (ITable table)
		{
			string objectTypeName = table.GetString ("object_type", null);
			if (objectTypeName == null)
				return null;
			Type type = null;
			if (!types.TryGetValue (objectTypeName, out type))
				return null;
			return type;
		}

		public T LoadObjectAs<T> (ITable table) where T : class
		{
			var type = GetObjectType (table);
			if (type == null)
				return Activator.CreateInstance (typeof(T)) as T;
			object loadedObject = Activator.CreateInstance (type);
			LoadObject (loadedObject, type, table);
			return (T)loadedObject;
		}

		public object LoadObject (Type defaultType, ITable table)
		{
			var type = GetObjectType (table);
			if (type == null)
				type = defaultType;
			object loadedObject = Activator.CreateInstance (type);
			LoadObject (loadedObject, type, table);
			return loadedObject;
		}

		public T LoadObjectAs<T> (Type type, ITable table) where T : class
		{
			object loadedObject = Activator.CreateInstance (type);
			LoadObject (loadedObject, type, table);
			return (T)loadedObject;
		}

		public void LoadObjectAs<T> (T obj, ITable table) where T : class
		{
			LoadObject (obj, obj.GetType (), table);
		}

		public void LoadObject<T> (T obj, ITable table) where T : class
		{
			LoadObject (obj, typeof(T), table);
		}


	}

	//	public class DefinedClassAttribute : Attribute
	//	{
	//
	//	}


}
