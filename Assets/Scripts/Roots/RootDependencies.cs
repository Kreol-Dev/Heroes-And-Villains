
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RootDependencies : Attribute
{
	public IDependency[] NeededRoots;
	public RootDependencies(params Type[] roots)
	{
		Type root = typeof(Root);
		List<IDependency> deps = new List<IDependency>();
		foreach ( var rootType in roots)
		{
			if (!rootType.IsSubclassOf(root))
				continue;
			deps.Add(Find.Root(rootType) as IDependency);
//			Debug.Log(rootType);
		}
		NeededRoots = deps.ToArray();
	}
}




