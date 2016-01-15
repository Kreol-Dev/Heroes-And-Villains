using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	[ECompName ("graphics")]
	public class EntityGraphics : EntityComponent
	{
		public override void LoadFromTable (ITable table)
		{
			ITable spriteTable = ((ITable)table.Get ("graphics")).Get ("sprite") as ITable;
			string spriteName = (string)spriteTable.Get (2);
			string packName = (string)spriteTable.Get (1);
			//Debug.LogWarningFormat ("{0} | {1}", packName, spriteName);
			SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer> ();
			renderer.sprite = Find.Root<Sprites> ().GetSprite (packName, spriteName);
			renderer.sortingOrder = 1;
		}

	}
}


