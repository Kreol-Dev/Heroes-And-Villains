using UnityEngine;
using System.Collections;
using Signals;
using System;


public abstract class Root : MonoBehaviour, IDependency
{
	Signal fullfill = new Signal();
	public Signal Fulfill { get { return fullfill; } }
	Dependencies deps;
	void Start()
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
		Debug.Log("Setup: " + gameObject.name);
		CustomSetup();
	}

	protected virtual void PreSetup() {}
	protected abstract void CustomSetup() ;
}

