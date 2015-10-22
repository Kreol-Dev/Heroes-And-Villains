using UnityEngine;
using System.Collections;
using Signals;
using System;


public abstract class Root : MonoBehaviour, IDependency
{
	public Signal Fulfill { get; internal set; }
	Dependencies deps;
	void Awake()
	{
		SetupDependecies();
		
	}

	void SetupDependecies ()
	{
		var type = this.GetType(); 
		if (!Attribute.IsDefined(type, typeof(RootDependencies)))
		{
			Setup ();
			return;
		}
		var attribDeps = Attribute.GetCustomAttribute(type, typeof(RootDependencies)) as RootDependencies;
		if (attribDeps.NeededRoots.Length == 0)
		{
			Setup ();
			return;
		}
		deps = new Dependencies(attribDeps.NeededRoots);
		deps.Fulfill.AddOnce(Setup);
	}

	void Setup()
	{
		CustomSetup();
	}

	protected abstract void CustomSetup() ;
}

