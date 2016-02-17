using UnityEngine;
using System.Collections;
using Signals;
using System;


public abstract class Root : MonoBehaviour, IDependency
{
	Signal fullfill = new Signal ();

	public Signal Fulfill { get { return fullfill; } }

	public bool Finished { get; internal set; }

	Dependencies deps;

	void Awake ()
	{
		fullfill.AddOnce (() =>
        {
			Debug.LogWarning ("Setup finished: " + gameObject.name);
			Finished = true;
        });
		PreSetup ();
	}

	void Start ()
	{
		SetupDependecies ();
		StartCoroutine (DelayedSetup ());
	}

	void SetupDependecies ()
	{
		Debug.Log ("Setup deps: " + gameObject.name);
		var type = this.GetType (); 
		if (Attribute.IsDefined (type, typeof(RootDependencies)))
		{
			var attribDeps = Attribute.GetCustomAttribute (type, typeof(RootDependencies)) as RootDependencies;
			if (attribDeps.NeededRoots.Length != 0)
			{
				deps = new Dependencies (attribDeps.NeededRoots);
				deps.Fulfill.AddOnce (() => StartCoroutine (SetupCoroutine ()));
			}
		}
	}

	bool fulfilled;

	IEnumerator SetupCoroutine ()
	{
		fulfilled = true;
		yield return null;
		Setup ();
	}

	void Setup ()
	{
		if (Finished)
			return;
		Debug.LogWarning ("Setup: " + gameObject.name);
		CustomSetup ();
	}

	protected virtual void PreSetup ()
	{
	}

	protected abstract void CustomSetup () ;

	IEnumerator DelayedSetup ()
	{
		yield return null;
		if (deps == null)
			Setup ();
	}

	public bool TrySetup ()
	{
		if (fulfilled)
			Setup ();
		return fulfilled;
	}
}

