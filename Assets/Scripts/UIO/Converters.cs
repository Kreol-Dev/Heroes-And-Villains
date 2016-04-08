using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UIO.BasicConverters;

namespace UIO
{
	public class Converters
	{
		Dictionary<Type, IConverter> converters = new Dictionary<Type, IConverter> ();
		Dictionary<Type, Type> genericConverters = new Dictionary<Type, Type> ();
		Definitions defs;
		Scribe scribe;

		public void AttachDefinitions (Definitions defs)
		{
			scribe = Scribes.Find ("CONVERTERS");
			this.defs = defs;
		}

		public IConverter GetConverter (Type type)
		{
			IConverter converter = null;
			if (!converters.TryGetValue (type, out converter))
			{

				if (converter == null)
				{
					converter = GetGenericConverter (type);
					if (converter == null)
					{
						if (type.IsValueType)
							converter = new NullConverter ();
						else
						{
							DefinitionConverter defConverter = new DefinitionConverter (type, defs);
							converter = defConverter;
						}
					}
				}
				converters.Add (type, converter);
				converter.Setup (this, defs);
			}
			scribe.LogFormatWarning ("{0}  |   {1}", type, converter.GetType ());
			return converter;
		}

		public void ReceiveConverters (IEnumerable<Type> converterTypes)
		{
			Type genericConvertersType = typeof(IGenericConverter);
			foreach (var converterType in converterTypes)
			{
				if (converterType == typeof(DefinitionConverter) || converterType.IsGenericType)
					continue;
				IConverter converter = (Activator.CreateInstance (converterType) as IConverter);
				try
				{
					if (genericConvertersType.IsAssignableFrom (converterType))
					{
						genericConverters.Add (converter.Type, converterType);
					} else
					{
						converters.Add (converter.Type, converter);
						converter.Setup (this, defs);
					}
				} catch (Exception e)
				{
					Debug.Log (converter.Type);
					Debug.LogWarningFormat ("{0} | {1} | ASASASAS", e.ToString (), converter.Type);
				}

			}
		}

		IConverter GetGenericConverter (Type type)
		{
			foreach (var genericPair in genericConverters)
				if (genericPair.Key.IsAssignableFrom (type))
				{
					IGenericConverter converter = Activator.CreateInstance (genericPair.Value) as IGenericConverter;
					((IConverter)converter).Setup (this, defs);
					converter.SpecifiedType = type;
					return converter as IConverter;
				}
			return null;
		}



	}
}


