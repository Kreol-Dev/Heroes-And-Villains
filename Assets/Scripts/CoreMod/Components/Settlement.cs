using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using Demiurg.Core.Extensions;


namespace CoreMod
{
    [ECompName ("settlement")]
    public class Settlement : EntityComponent
    {
        public int Population;
        public string Race;

        public override void LoadFromTable (ITable table)
        {
            ITable compTable = table.Get ("settlement") as ITable;
            Population = (int)(double)compTable.Get ("population");
            Race = (string)compTable.Get ("race");
        }
    }
}


