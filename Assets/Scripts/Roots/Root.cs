using UnityEngine;
using System.Collections;
using Signals;
using System;


public abstract class Root : MonoBehaviour, IDependency
{
	Signal fullfill = new Signal();
	public Signal Fulfill { get { return fullfill; } }
	Dependencies deps;
	void Awake()
	{
		fullfill.AddOnce(() => {Debug.Log("Setup finished: " + gameObject.name);});
		PreSetup();
	}
	void Start()
	{
		SetupDependecies();
		StartCoroutine(DelayedSetup());
	}

	void SetupDependecies ()
	{
		Debug.Log("Setup deps: " + gameObject.name);
		var type = this.GetType(); 
		if (Attribute.IsDefined(type, typeof(RootDependencies)))
		{
			var attribDeps = Attribute.GetCustomAttribute(type, typeof(RootDependencies)) as RootDependencies;
			if (attribDeps.NeededRoots.Length != 0)
			{
				deps = new Dependencies(attribDeps.NeededRoots);
				deps.Fulfill.AddOnce(Setup);
			}
		}
	}

	void Setup()
	{
		Debug.Log("Setup: " + gameObject.name);
		CustomSetup();
	}

	protected virtual void PreSetup() {}
	protected abstract void CustomSetup() ;

	IEnumerator DelayedSetup()
	{
		yield return null;
		if (deps == null)
			Setup();
	}
}

