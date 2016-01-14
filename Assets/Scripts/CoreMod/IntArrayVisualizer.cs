
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;
using Demiurg.Core;


namespace CoreMod
{
    public class IntArrayVisualizer : ArrayVisualiser
    {
        [AInput ("main")]
        int[,] main;

        Color FindColor (int value)
        {
            for (int i = 0; i < Levels.Count - 1; i++)
            {
                //Debug.LogFormat ("{0} {1} {2}", levels [i + 1].Level, value, levels [i].Level);
                if (Levels [i + 1].Level >= value && Levels [i].Level <= value)
                    return Color.Lerp (Levels [i].Color, Levels [i + 1].Color, Mathf.InverseLerp (Levels [i].Level, Levels [i + 1].Level, value));
            }
            return Color.clear;
        }

        protected override Sprite CreateSprite ()
        {
            Texture2D texture = new Texture2D (main.GetLength (0), main.GetLength (1));
            for (int i = 0; i < main.GetLength (0); i++)
                for (int j = 0; j < main.GetLength (1); j++)
                {
                    texture.SetPixel (i, j, FindColor (main [i, j]));
                }
            texture.Apply ();
            return Sprite.Create (texture, Rect.MinMaxRect (0, 0, main.GetLength (0), main.GetLength (1)), Vector2.zero, 1f);
        }
    }

}




