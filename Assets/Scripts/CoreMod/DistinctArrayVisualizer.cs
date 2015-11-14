
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg;


namespace CoreMod
{
    public class DistinctArrayVisualizer : ArrayVisualiser
    {

        NodeInput<int[,]> main;
        NodeOutput<Texture2D> mainO;
        struct LevelPair
        {
            public int Level;
            public Color Color;
        }
        LevelPair[] levels;
        BoolParam random;
        protected override void SetupIOP ()
        {
            base.SetupIOP ();
            main = Input<int[,]> ("main");
            mainO = Output<Texture2D> ("main");
            random = Config<BoolParam> ("random");
        }
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
            Texture2D texture = new Texture2D (main.Content.GetLength (0), main.Content.GetLength (1));
            if (!random.Content)
            {
                levels = new LevelPair[Levels.Content.Length];
                for (int i = 0; i < Levels.Content.Length; i++)
                {
                    levels [i].Level = (int)Levels.Content [i].Level.Content;
                    levels [i].Color = new Color (
                        Levels.Content [i].Red,
                        Levels.Content [i].Green,
                        Levels.Content [i].Blue);
                    
                }
                for (int i = 0; i < main.Content.GetLength(0); i++)
                    for (int j = 0; j < main.Content.GetLength(1); j++)
                    {
                        texture.SetPixel (i, j, FindColor (main.Content [i, j]));
                    }
            }
            else
            {
                Dictionary<int, Color> colors = new Dictionary<int, Color> ();
                for (int i = 0; i < main.Content.GetLength(0); i++)
                    for (int j = 0; j < main.Content.GetLength(1); j++)
                    {
                        Color color = Color.white;
                        if (!colors.TryGetValue (main.Content [i, j], out color))
                        {
                            color = new Color ((float)Random.NextDouble (), (float)Random.NextDouble (), (float)Random.NextDouble ());
                            colors.Add (main.Content [i, j], color);
                        }
                        texture.SetPixel (i, j, color);
                    }
            }
            texture.Apply ();
            mainO.Finish (texture);
            return Sprite.Create (texture, Rect.MinMaxRect (0, 0, main.Content.GetLength (0), main.Content.GetLength (1)), Vector2.zero);
        }
    }

}




