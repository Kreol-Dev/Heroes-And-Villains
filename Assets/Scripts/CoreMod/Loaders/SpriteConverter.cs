using UnityEngine;
using System.Collections;
using UIO;
using System;

namespace CoreMod
{
	public class SpriteConverter : IConverter<Sprite>
	{
		public override object Load (object key, ITable table, bool reference)
		{
			if (reference == false)
				return null;
			var sprites = Find.Root<Sprites> ();
			ITable spriteRefTable = table.GetTable (key);
			Sprite sprite = sprites.GetSprite (spriteRefTable.GetString (1), spriteRefTable.GetString (2));
			return sprite;
		}

		public override void Save (object key, ITable table, object obj, bool reference)
		{
			throw new NotImplementedException ();
		}
	}


}
