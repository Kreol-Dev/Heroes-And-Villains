using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;

namespace CoreMod
{
	[ECompName ("graphics")]
	public class EntityGraphics : EntityComponent
	{

		public override void PostCreate ()
		{
			
		}

		
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
			int priority = table.GetInt ("priority");
			ITable spriteTable = table.GetTable ("sprite");
			spriteName = spriteTable.GetString (2);
			packName = spriteTable.GetString (1);
			//Debug.LogWarningFormat ("{0} | {1}", packName, spriteName);
			SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer> ();
			renderer.sprite = Find.Root<Sprites> ().GetSprite (packName, spriteName);
			renderer.sortingOrder = 1;
		}

	}

	public class GraphicsSharedData
	{
		public Sprite Sprite { get; internal set; }

		public int Priority { get; internal set; }

		public GraphicsSharedData (Sprite sprite, int priority)
		{
			Sprite = sprite;
			Priority = priority;
		}
	}

	public class TiledGraphicsLayer : GOLayer<EntityGraphics>
	{
	}


}


