using UnityEngine;
using System.Collections;
using Signals;
using System;

namespace Demiurg
{
	public abstract class NodeInput
	{
		public abstract void ConnectTo(CreationNode node, string outputName);
	}
	public class NodeInput<T> : NodeInput
	{
		public T Content;
		public string Name { get; internal set; }
		Signal finishSignal = new Signal();
		public NodeInput(string name, Action onFinish)
		{
			Name = name;
			finishSignal.AddOnce(onFinish);
		}
		
		public override void ConnectTo(CreationNode node, string outputName)
		{
			NodeOutput<T> output = node.GetOutput<T>(outputName);
			output.OnFinish(content => { Content = content; finishSignal.Dispatch();});
		}
	}
}


