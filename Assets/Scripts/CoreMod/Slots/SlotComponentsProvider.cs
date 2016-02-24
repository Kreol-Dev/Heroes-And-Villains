using UnityEngine;
using System.Collections;
using CoreMod;
using UIO;
using MoonSharp.Interpreter;

[AShared]
public class SlotComponentsProvider
{
	public GameObject GO;
	SlotComponentsRoot components;

	public SlotComponentsProvider ()
	{
		components = Find.Root<SlotComponentsRoot> ();
	}

	public void ShowTable (Table table)
	{
		foreach (var entry in table.Pairs)
			Debug.LogFormat ("{0} | {1}", entry.Key, entry.Value);
	}

	public SlotComponent Get (int componentIndex)
	{
		if (GO != null)
			return GO.GetComponent (components.GetTypeByID (componentIndex)) as SlotComponent;
		else
			return null;
	}

}

