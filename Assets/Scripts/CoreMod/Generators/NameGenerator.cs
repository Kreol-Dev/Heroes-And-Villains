using UnityEngine;
using System.Collections;

public static class NameGenerator
{
	static int _factionsCount = 0;

	public static string GenerateFactionName ()
	{
		return "Faction " + _factionsCount++;
	}

}

