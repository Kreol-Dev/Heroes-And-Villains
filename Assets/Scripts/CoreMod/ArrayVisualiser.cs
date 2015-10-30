
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
namespace CoreMod
{
	public abstract class ArrayVisualiser : CreationNode
	{
		
		protected class ColorLevel
		{
			public FloatParam Level = new FloatParam("level");
			public FloatParam Red = new FloatParam("red");
			public FloatParam Green = new FloatParam("green");
			public FloatParam Blue = new FloatParam("blue");
		}
		protected GlobalArrayParam<ColorLevel> Levels;
		public ArrayVisualiser ()
		{
			Levels = Config<GlobalArrayParam<ColorLevel>>("levels");
		}
		protected sealed override void Work ()
		{
			GameObject go = new GameObject(Name);
			go.AddComponent<SpriteRenderer>().sprite = CreateSprite();

		}
		protected abstract Sprite CreateSprite();
	}
}





