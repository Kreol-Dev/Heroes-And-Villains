using UnityEngine;
using System.Collections;
using System;

namespace UIO
{
	public abstract class IConverter
	{
		protected static Converters Converters;
		protected static Definitions Definitions;

		public void Setup (Converters converters, Definitions definitions)
		{
			Converters = converters;
			Definitions = definitions;
		}

		public abstract Type Type { get; }

		public abstract object Load (object key, ITable table, bool reference);

		public abstract void Save (object key, ITable table, object obj, bool reference);
	}

	public abstract class IConverter<T> : IConverter
	{
		static readonly Type convertedType = typeof(T);

		public sealed override Type Type { get { return convertedType; } }
	}

	public interface IReferenceConverter
	{
		object Load (object fromObject);
	}

	public interface IGenericConverter
	{
		Type SpecifiedType { get; set; }
	}


}


