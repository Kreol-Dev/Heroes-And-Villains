using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	[ECompName ("graphics")]
	public class EntityGraphics : EntityComponent
	{
		public override void CopyTo (GameObject go)
		{
			EntityGraphics eg = go.AddComponent<EntityGraphics> ();
			SpriteRenderer renderer = go.AddComponent<SpriteRenderer> ();
			renderer.sprite = Find.Root<Sprites> ().GetSprite (packName, spriteName);
			renderer.sortingOrder = 1;
		}

		string spriteName;
		string packName;

		public override void LoadFromTable (ITable table)
		{
			ITable spriteTable = table.Get ("sprite") as ITable;
			spriteName = (string)spriteTable.Get (2);
			packName = (string)spriteTable.Get (1);
			//Debug.LogWarningFormat ("{0} | {1}", packName, spriteName);
			SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer> ();
			renderer.sprite = Find.Root<Sprites> ().GetSprite (packName, spriteName);
			renderer.sortingOrder = 1;
		}

	}
}


