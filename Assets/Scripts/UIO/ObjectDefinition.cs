using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UIO.BasicConverters;

namespace UIO
{
	public class ObjectDefinition
	{
		FieldDefinition[] definitions;
		static Converters Converters;

		public void LoadObject (object obj, ITable objectTable)
		{
			for (int i = 0; i < definitions.Length; i++)
				definitions [i].LoadField (obj, objectTable);
		}

		public void SaveObject (ITable objectTable, object obj)
		{
			for (int i = 0; i < definitions.Length; i++)
				definitions [i].SaveField (obj, objectTable);
		}

		public ObjectDefinition (Type type, Converters converters)
		{
			Converters = converters;
			var fields = from field in type.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			             let attr = DefinedAttributeID (field)
			             where attr != null
			             select new FieldDefinition (field, attr, Converters.GetConverter (field.FieldType));
			var properties = from field in type.GetProperties (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			                 let attr = DefinedAttributeID (field)
			                 where attr != null
			                 select new FieldDefinition (field, attr, Converters.GetConverter (field.PropertyType));
			int defsSize = fields.Count () + properties.Count ();
			definitions = new FieldDefinition[defsSize];
			int defID = 0;
			foreach (var field in fields)
				definitions [defID++] = field;
			foreach (var property in properties)
				definitions [defID++] = property;
		}


		static readonly Type definedAttrib = typeof(DefinedAttribute);

		DefinedAttribute DefinedAttributeID (MemberInfo fieldInfo)
		{
			DefinedAttribute attr = Attribute.GetCustomAttribute (fieldInfo, definedAttrib) as DefinedAttribute;
			if (attr == null)
				return null;
			return attr;
		}


	}

	public struct FieldDefinition
	{
		readonly object id;
		readonly bool isProperty;
		readonly MemberInfo member;
		readonly IConverter converter;
		readonly bool isReference;

		public FieldDefinition (MemberInfo member, DefinedAttribute attr, IConverter loader)
		{
			this.id = attr.ID;
			this.isReference = attr.ContainsReferences;
			this.member = member;
			if (member is PropertyInfo)
				isProperty = true;
			else
				isProperty = false;
			this.converter = loader;
		}

		public void LoadField (object toObject, ITable fromTable)
		{
			if (converter == null)
				return;
			if (isProperty)
			{
				((PropertyInfo)member).SetValue (toObject, converter.Load (id, fromTable, isReference), null);
			} else
			{
				((FieldInfo)member).SetValue (toObject, converter.Load (id, fromTable, isReference));
			}
		}

		public void SaveField (object fromObject, ITable toTable)
		{
			if (converter == null)
				return;
			if (isProperty)
			{
				converter.Save (id, toTable, ((PropertyInfo)member).GetValue (fromObject, null), isReference);
			} else
			{
				converter.Save (id, toTable, ((FieldInfo)member).GetValue (fromObject), isReference);
			}
		}

	}

	public class DefinedAttribute : Attribute
	{
		public object ID { get; internal set; }

		public bool ContainsReferences { get; internal set; }

		public DefinedAttribute (string id, bool containsReferences = false)
		{
			ID = id;
			ContainsReferences = containsReferences;
		}

		public DefinedAttribute (int id, bool containsReferences = false)
		{
			ID = id;
			ContainsReferences = containsReferences;
		}
	}


}



