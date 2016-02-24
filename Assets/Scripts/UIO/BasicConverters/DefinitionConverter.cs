using UnityEngine;
using System.Collections;
using System;

namespace UIO.BasicConverters
{
	public class DefinitionConverter : IConverter<object>
	{
		public DefinitionConverter (Type type, Definitions definitions)
		{
			this.SpecifiedType = type;
			Definitions = definitions;
		}

		Type SpecifiedType;

		static Definitions Definitions;

		public override object Load (object key, ITable table, bool reference)
		{
			ITable objectTable = table.GetTable (key);
			Type objectType = Definitions.GetObjectType (objectTable);
			if (objectType == null)
				return Definitions.LoadObjectAs<object> (SpecifiedType, objectTable);
			else if (objectType.IsSubclassOf (SpecifiedType))
				return Definitions.LoadObjectAs<object> (objectType, objectTable);
			else
				return Activator.CreateInstance (SpecifiedType);
				
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	
	}


	public class NullConverter : IConverter
	{
		public override object Load (object key, ITable table, bool reference)
		{
			return null;
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
		}

		public override Type Type {
			get
			{
				return typeof(object);
			}
		}
		
	}
}


