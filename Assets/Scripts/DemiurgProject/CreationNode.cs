using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaTableEntries;
using System;

public abstract class CreationNode 
{
	Scribe scribe = Scribes.Find("NodesScribe");
	public string Name { get; internal set; }
	Dictionary<string, object> inputs = new Dictionary<string, object>();
	Dictionary<string, object> outputs = new Dictionary<string, object>();
	List<NodeParam> parameters = new List<NodeParam>();

	InputsCounter counter;
	class InputsCounter 
	{
		Action action;
		int counter = 0;
		public InputsCounter(Action onAllInputs)
		{
			action = onAllInputs;
		}
		public void AddInput()
		{
			counter++;
		}
		public void SatisfyInput()
		{
			counter--;
			if (counter == 0)
				action();
		}
	}
	protected NodeInput<T> Input<T>(string name)
	{
		NodeInput<T> input = new NodeInput<T>(name, counter.SatisfyInput);
		inputs.Add(name, input);
		counter.AddInput();
		return input;
	}

	protected NodeOutput<T> Output<T>(string name)
	{
		NodeOutput<T> output = new NodeOutput<T>(name);
		outputs.Add(name, output);
		return output;
	}

	public void Init(string name, Table paramsTable)
	{
		counter = new InputsCounter(Work);
		Name = name;
		LoadParams(paramsTable);
	}
	protected virtual void LoadParams(Table paramsTable){}

	public NodeOutput<T> GetOutput<T>(string name)
	{
		object protoOutput = null;
		outputs.TryGetValue(name, out protoOutput);
		if (protoOutput == null)
		{
			scribe.LogFormat("Can't find output {0} in {1} ({2})", name, Name, this.GetType());
		}
		NodeOutput<T> output = protoOutput as NodeOutput<T>;
		if (output == null)
		{
			scribe.LogFormat("Can't cast output {0} in {1} ({2}) to type {3}", name, Name, this.GetType(), typeof(NodeOutput<T>));
		}
		return null;
	}

	protected abstract void Work();
}

