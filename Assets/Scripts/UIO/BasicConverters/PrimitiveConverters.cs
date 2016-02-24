using UnityEngine;
using System.Collections;
using System;


namespace UIO.BasicConverters
{
	public class IntConverter : IConverter<int>, IReferenceConverter
	{
		public override object Load (object key, ITable table, bool reference)
		{
			return table.GetInt (key);
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}

		public object Load (object fromObject)
		{
			return fromObject;
		}
	}

	public class FloatConverter : IConverter<float>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			return table.GetFloat (key);
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}

	public class DoubleConverter : IConverter<double>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			return table.GetDouble (key);
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}

	public class BoolConverter : IConverter<bool>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			return table.GetBool (key);
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}

	public class StringConverter : IConverter<string>, IReferenceConverter
	{
		public override object Load (object key, ITable table, bool reference)
		{
			return table.GetString (key);
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}

		public object Load (object fromObject)
		{
			return fromObject;
		}
	}

	public class CallbackConverter : IConverter<ICallback>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			return table.GetCallback (key);
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}

	public class TableConverter : IConverter<ITable>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			return table.GetTable (key);
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}

	public class EnumConverter : IConverter<Enum>, IGenericConverter
	{
		public Type SpecifiedType { get; set; }

		public override object Load (object key, ITable table, bool reference)
		{
			try
			{
				int enumValue = table.GetInt (key);
				return Enum.Parse (SpecifiedType, enumValue.ToString ());
			} catch
			{
				string enumString = table.GetString (key);
				return Enum.Parse (SpecifiedType, enumString);
			}
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new System.NotImplementedException ();
		}
		
	}
}

