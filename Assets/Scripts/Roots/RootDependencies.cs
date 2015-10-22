
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RootDependencies : Attribute
{
	public IDependency[] NeededRoots;
	public RootDependencies(params Root[] roots)
	{
		NeededRoots = roots;
	}
}




