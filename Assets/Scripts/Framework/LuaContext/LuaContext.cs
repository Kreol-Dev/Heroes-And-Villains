using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Demiurg.Core.Extensions;

public class LuaContext : Root
{
    Scribe scribe;
	
    Dictionary<string, ITable> tables;

    protected override void PreSetup ()
    {
        base.PreSetup ();
		
        scribe = Scribes.Register ("LuaContext");
        tables = new Dictionary<string, ITable> ();
    }

    protected override void CustomSetup ()
    {
        Fulfill.Dispatch ();
    }
	
	
	
	
	
}

