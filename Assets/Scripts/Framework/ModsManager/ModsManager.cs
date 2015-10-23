
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RootDependencies(typeof(LuaContext))]
public class ModsManager : Root
{
	protected override void PreSetup ()
	{
		base.PreSetup ();
		SearchForMods();
		DetermineActiveMods();
		ReadActiveMods();
		DetermineOrder();
		InitMods();
	}

	protected override void CustomSetup ()
	{
		Fulfill.Dispatch();
	}
	void SearchForMods ()
	{

	}

	void DetermineActiveMods ()
	{

	}

	void ReadActiveMods ()
	{

	}

	void DetermineOrder ()
	{

	}

	void InitMods ()
	{

	}

	public List<string> GetFiles(string templatePath)
	{
		return new List<string>();
	}

}




