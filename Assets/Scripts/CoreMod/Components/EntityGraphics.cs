using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using UIO;

namespace CoreMod
{
	[AShared]
	[ECompName ("graphics")]
	public class EntityGraphics : EntityComponent
	{
		public override EntityComponent CopyTo (GameObject go)
		{
			EntityGraphics eg = go.AddComponent<EntityGraphics> ();
			SpriteRenderer renderer = go.AddComponent<SpriteRenderer> ();
			renderer.sprite = Find.Root<Sprites> ().GetSprite (packName, spriteName);
			renderer.sortingOrder = 1;
			return eg;
		}

		string spriteName;
		string packName;

		public override void LoadFromTable (ITable table)
		{
			ITable spriteTable = table.GetTable ("sprite");
			spriteName = spriteTable.GetString (2);
			packName = spriteTable.GetString (1);
			//Debug.LogWarningFormat ("{0} | {1}", packName, spriteName);
			SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer> ();
			renderer.sprite = Find.Root<Sprites> ().GetSprite (packName, spriteName);
			renderer.sortingOrder = 1;
		}

		public override void PostCreate ()
		{

		}

		protected override void PostDestroy ()
		{

		}
	}
}


