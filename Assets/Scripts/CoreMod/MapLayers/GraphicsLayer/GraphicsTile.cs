using UnityEngine;
using System.Collections;
using System;

namespace CoreMod
{
	public class GraphicsTile
	{
		public Sprite Sprite { get; internal set; }

		public int Priority { get; internal set; }

		public string Name { get; internal set; }

		public GraphicsTile (Sprite sprite, int priority, string name)
		{
			Sprite = sprite;
			Priority = priority;
			Name = name;
		}
	}
}


