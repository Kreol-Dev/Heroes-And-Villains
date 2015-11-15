using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

public class Sprites : Root
{ 
    Dictionary<string, Dictionary<string, Sprite>> tree = new Dictionary<string, Dictionary<string, Sprite>> ();
    Sprite error = Sprite.Create (new Texture2D (2, 2), Rect.MinMaxRect (0, 0, 2, 2), Vector2.zero);
    public Sprite GetSprite (string packName, string spriteName)
    {
        Dictionary<string, Sprite> pack = null;
        tree.TryGetValue (packName, out pack);
        if (pack == null)
            return error;
        Sprite sprite = null;
        pack.TryGetValue (spriteName, out sprite);
        if (sprite == null)
            return error;
        return sprite;
    }
    protected override void PreSetup ()
    {
        string[] paths = Directory.GetFiles ("Mods\\CoreMod\\Sprites");
        for (int i = 0; i < paths.Length; i++)
        {
            string ext = Path.GetExtension (paths [i]);
            

            if (ext == ".lua")
            {
                string name = Path.GetFileNameWithoutExtension (paths [i]);
                Texture2D texture = new Texture2D (2, 2);
                texture.filterMode = FilterMode.Point;
                texture.LoadImage (File.ReadAllBytes ("Mods\\CoreMod\\Sprites\\" + name + ".png"));
                Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite> ();
                tree.Add (name, sprites);
                Script script = new Script ();
                script.Options.ScriptLoader = new FileSystemScriptLoader ();
                script.Globals ["sprites"] = new Table (script);
                script.DoFile (paths [i], script.Globals ["sprites"] as Table);
                foreach (var pair in ((Table)script.Globals["sprites"]).Pairs)
                {
                    Table spriteTable = pair.Value.Table;
                    int minX = (int)(double)((Table)spriteTable ["left_top_corner"]) [1];
                    int minY = (int)(double)((Table)spriteTable ["left_top_corner"]) [2];
                    int maxX = (int)(double)((Table)spriteTable ["right_bottom_corner"]) [1];
                    int maxY = (int)(double)((Table)spriteTable ["right_bottom_corner"]) [2];
                    Sprite sprite = Sprite.Create (texture, Rect.MinMaxRect (minX, minY, maxX, maxY), Vector2.zero, 32f);
                    Debug.LogWarningFormat ("{0} {1}", name, pair.Key.ToPrintString ());
                    sprites.Add (pair.Key.ToPrintString (), sprite);
                }
            }
                
        }
    }
    protected override void CustomSetup ()
    {
        Fulfill.Dispatch ();
    }
}

