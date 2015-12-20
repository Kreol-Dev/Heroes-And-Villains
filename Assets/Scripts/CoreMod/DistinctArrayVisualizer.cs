
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using Demiurg.Core;
using System;


namespace CoreMod
{
    public class DistinctArrayVisualizer  : ArrayVisualiser
    {
        [AInput ("main")]
        int[,] main;
        [AOutput ("main")]
        Texture2D mainO;
        [AConfig ("random")]
        bool random;

        struct LevelPair
        {
            public int Level;
            public Color Color;
        }

        LevelPair[] levels;

        Color FindColor (int value)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels [i].Level == value)
                    return levels [i].Color;
            }
            return Color.clear;
        }

        protected override Sprite CreateSprite ()
        {
            levels = new LevelPair[Levels.Count];
            for (int i = 0; i < Levels.Count; i++)
                levels [i] = new LevelPair () {
                    Color = Levels [i].Color,
                    Level = Convert.ToInt32 (Levels [i].Level)
                };
            int sizeX = main.GetLength (0);
            int sizeY = main.GetLength (1);
            Texture2D texture = new Texture2D (sizeX, sizeY);
            if (random == true)
            {
                Debug.LogWarning ("Random discrete colours");
                Dictionary<int, Color> colors = new Dictionary<int, Color> ();
                for (int i = 0; i < sizeX; i++)
                    for (int j = 0; j < sizeY; j++)
                    {
                        Color color = Color.white;
                        if (!colors.TryGetValue (main [i, j], out color))
                        {
                            color = new Color ((float)Random.NextDouble (), (float)Random.NextDouble (), (float)Random.NextDouble ());
                            colors.Add (main [i, j], color);
                        }
                        texture.SetPixel (i, j, color);
                    }
            }
            else
            {
                for (int i = 0; i < sizeX; i++)
                    for (int j = 0; j < sizeY; j++)
                    {
                        Color color = FindColor (main [i, j]);
                        texture.SetPixel (i, j, color);
                    }
            }

            texture.Apply ();
            mainO = texture;
            return Sprite.Create (texture, Rect.MinMaxRect (0, 0, sizeX, sizeY), Vector2.zero, 1f);
        }
    }

}




