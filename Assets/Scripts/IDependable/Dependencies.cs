
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Signals;

public class Dependencies : IDependency
{
	Signal fullfill = new Signal();
	public Signal Fulfill { get { return fullfill; } }
	int dependenciesCount; 
	public Dependencies (params IDependency[] dependencies)
	{
		foreach ( var dependency in dependencies )
			dependency.Fulfill.AddOnce(DependencyFulfilled);			
		dependenciesCount = dependencies.Length;
	}
	void DependencyFulfilled()
	{
		dependenciesCount--;
		if (dependenciesCount == 0)
			Fulfill.Dispatch();
	}
}




