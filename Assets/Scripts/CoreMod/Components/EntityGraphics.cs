using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;


namespace CoreMod
{
    public class EntityGraphics : EntityComponent
    {
        public override void LoadFromTable (Table table)
        {
            Table spriteTable = ((Table)table ["graphics"]) ["sprite"] as Table;
            string spriteName = (string)spriteTable [2];
            string packName = (string)spriteTable [1];
            //Debug.LogWarningFormat ("{0} | {1}", packName, spriteName);
            gameObject.AddComponent<SpriteRenderer> ().sprite = Find.Root<Sprites> ().GetSprite (packName, spriteName);
        }

    }
}


