
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using System;


namespace CoreMod
{
	public class FloatArrayVisualizer : ArrayVisualiser
	{
		NodeInput<float[,]> main;
		struct LevelPair
		{
			public float Level;
			public Color Color;
		}
		LevelPair[] levels;
		public FloatArrayVisualizer () : base()
		{

			main = Input<float[,]>("main");
		}
		Color FindColor(float value)
		{
			for (int i = 0; i < levels.Length - 1; i++)
			{
				if (levels[i + 1].Level >= value && levels[i].Level <= value)
					return Color.Lerp(levels[i].Color, levels[i + 1].Color, Mathf.InverseLerp(levels[i].Level, levels[i + 1].Level, value));
			}
			return Color.clear;
		}
		protected override Sprite CreateSprite ()
		{
			levels = new LevelPair[Levels.Content.Length];
			for (int i = 0; i < Levels.Content.Length; i++ )
			{
				levels[i].Level = Levels.Content[i].Level;
				levels[i].Color = new Color(
					Levels.Content[i].Red,
					Levels.Content[i].Green,
					Levels.Content[i].Blue);
				
			}
			Texture2D texture = new Texture2D(main.Content.GetLength(0), main.Content.GetLength(1));
			for (int i = 0; i < main.Content.GetLength(0); i++)
				for (int j = 0; j < main.Content.GetLength(1); j++)
			{
				texture.SetPixel(i, j, FindColor(main.Content[i, j]));
			}
			texture.Apply();
			return Sprite.Create(texture, Rect.MinMaxRect(0, 0, main.Content.GetLength(0), main.Content.GetLength(1)), Vector2.zero);
		}
	}
}





