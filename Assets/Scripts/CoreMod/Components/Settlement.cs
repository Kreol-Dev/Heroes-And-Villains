using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;


namespace CoreMod
{
    public class Settlement : EntityComponent
    {
        public int Population;
        public string Race;
        public override void LoadFromTable (Table table)
        {
            Table compTable = ((Table)table ["settlement"]);
            Population = (int)(double)compTable ["population"];
            Race = (string)compTable ["race"];
        }
    }
}


