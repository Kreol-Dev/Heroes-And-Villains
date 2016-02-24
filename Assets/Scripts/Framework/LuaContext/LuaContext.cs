using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIO;

public class LuaContext : Root
{

	protected override void PreSetup ()
	{
		base.PreSetup ();
		
       
	}

	protected override void CustomSetup ()
	{
		Fulfill.Dispatch ();
	}
	
	
	
	
	
}

