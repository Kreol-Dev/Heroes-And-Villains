using UnityEngine;
using System.Collections;
using System;

namespace UIO.BasicConverters
{
	public class DictionaryConverter : IConverter<IDictionary>, IGenericConverter
	{
		public System.Type SpecifiedType { get; set; }

		
		public override object Load (object key, ITable table, bool reference)
		{
			IDictionary dict = Activator.CreateInstance (SpecifiedType) as IDictionary;
			var args = SpecifiedType.GetGenericArguments ();
			IReferenceConverter keysConverter = Converters.GetConverter (args [0]) as IReferenceConverter;
			if (keysConverter == null)
				return dict;
			IConverter valueConverter = null;
			if (!args [1].IsClass || args [1] == typeof(string))
			{
				valueConverter = Converters.GetConverter (args [1]);
				var dictTable = table.GetTable (key);
				foreach (var pairKey in dictTable.GetKeys())
				{
					object dictKey = keysConverter.Load (pairKey);
					object dictValue = valueConverter.Load (pairKey, dictTable, reference);
					dict.Add (dictKey, dictValue);
				}

			} else
			{
				var dictTable = table.GetTable (key);
				if (reference)
				{
					valueConverter = Converters.GetConverter (args [1]);
					foreach (var pairKey in dictTable.GetKeys())
					{

						object dictKey = keysConverter.Load (pairKey);
						object dictValue = valueConverter.Load (pairKey, dictTable, reference);
						dict.Add (dictKey, dictValue);
					}
				} else
					foreach (var pairKey in dictTable.GetKeys())
					{

						object dictKey = keysConverter.Load (pairKey);
						object dictValue = Definitions.LoadObject (args [0], dictTable.GetTable (pairKey));
						dict.Add (dictKey, dictValue);
					}
			}
			return dict;
				
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new System.NotImplementedException ();
		}
		
	}

	public class ListConverter : IConverter<IList>, IGenericConverter
	{
		public System.Type SpecifiedType { get; set; }

		public override object Load (object key, ITable table, bool reference)
		{
			IList list = Activator.CreateInstance (SpecifiedType) as IList;
			var args = SpecifiedType.GetGenericArguments ();

			var listTable = table.GetTable (key);
			if (!args [0].IsClass || args [0] == typeof(string))
			{
				var valueConverter = Converters.GetConverter (args [0]);
				foreach (var listKey in listTable.GetKeys())
				{
					object value = valueConverter.Load (listKey, listTable, reference);
					list.Add (value);
				}
				return list;
			}

			if (reference)
			{
				var valueConverter = Converters.GetConverter (args [0]);
				foreach (var listKey in listTable.GetKeys())
				{
					object value = valueConverter.Load (listKey, listTable, reference);
					list.Add (value);
				}
			} else
			{
				foreach (var listKey in listTable.GetKeys())
				{
					object value = Definitions.LoadObject (args [0], listTable.GetTable (listKey));
					list.Add (value);
				}
			}
			return list;
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new System.NotImplementedException ();
		}
	}
	
}
