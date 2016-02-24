using UnityEngine;
using System.Collections;
using System;
using UIO;

namespace CoreMod
{
	public class ColorLoader : IConverter<Color>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			ITable colorTable = table.GetTable (key);
			return new Color (colorTable.GetFloat (1), colorTable.GetFloat (2), colorTable.GetFloat (3));
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}

	}
}



